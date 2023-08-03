using internshipProject1.Entities;
using internshipProject1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Project.Controllers
{
    [Authorize]
    public class AccountController : Controller

    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login() /*Login sayfamız icin*/
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)

        {


            if (ModelState.IsValid)
            {
                string hashedPassword = MD5HashedString(model.Password);

                User user = _databaseContext.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == hashedPassword);   /*kontrol yapmak icin ve kayıt varsa bize döncek bi tane*/

                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError(nameof(model.Username), "User is locked");
                        return View(model);
                    }

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? string.Empty));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role, string.Empty));
                    claims.Add(new Claim("Username", user.Username));

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    /*httpcontext sınıfı icerisinde hazır gelen ve bizi içeriye login edicek bir SignnAsync methodu var*/

                    return RedirectToAction("Index", "Home");


                }
                else
                {
                    ModelState.AddModelError("", "Username or password is incorrect");
                }






            }
            return View(model); /*eğer hata varsa viewla bu modeli geri gönderiyor olcam sayfada hatalar görünsün diye*/
        }

        private string MD5HashedString(string s)
        {
            string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
            string salted = s + md5Salt;
            string hashed = salted.MD5();
            return hashed;
        }

        [AllowAnonymous]
        public IActionResult Register() /*//Register sayfamız icin*/
        {

            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model) /*//Register sayfamız icin*/
        {
            if (ModelState.IsValid)
            {

                if (_databaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists.");
                    return View(model);
                }


                string hashedPassword = MD5HashedString(model.Password);

                User user = new() /*data validse bir user nesnesi olustur*/
                {
                    Username = model.Username,
                    Password = hashedPassword

                };
                _databaseContext.Users.Add(user);  /*yeni nesnemi user tablosuna ekledim*/
                int affectedRowCount = _databaseContext.SaveChanges();  /*savededigimde ise insert oluyor*/

                if (affectedRowCount == 0)
                {
                    ModelState.AddModelError("", "User can not be added.");
                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }
            }

            return View(model);


        }


        public IActionResult Profile() /*//Profil sayfamız icin*/
        {
            ProfileInfoLoader();

            return View();
        }

        public void ProfileInfoLoader()
        {
            Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

            ViewData["FullName"] = user.FullName;
            ViewData["ProfileImage"] = user.ProfileImageFileName;                         /*imageın adını da gönderebiliriz*/
        }

        [HttpPost]
        public IActionResult ProfileChangeFullName([Required][StringLength(50)] string? fullName)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

                user.FullName = fullName;
                _databaseContext.SaveChanges();

                ViewData["FullName"] = "FullNameSaved";

            }

            ProfileInfoLoader();
            return View("Profile");

        }

        [HttpPost]
        public IActionResult ProfileChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

                string hashedPassword = MD5HashedString(password);

                user.Password = hashedPassword;
                _databaseContext.SaveChanges();

                ViewData["result"] = "PasswordChanged";
            }

            ProfileInfoLoader();
            return View("Profile");

        }

        [HttpPost]
        public IActionResult ProfileChangeImage([Required] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

                //p_guid.jpg
                string fileName = $"p_{userid}.jpg";     /*dosya adı olusturduk*/

                Stream stream = new FileStream($"wwwroot/uploads/{fileName}", FileMode.OpenOrCreate);   /*dosyayı yüklemek icin stream nesnesini kullanıyoruz, file stream (path,mod)*/

                file.CopyTo(stream);     /* Resmi kopyalamamız icin sunucuya*/

                stream.Close();
                stream.Dispose();         

                user.ProfileImageFileName = fileName;
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Profile));
            }

            ProfileInfoLoader();
            return View("Profile");

        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));   /*Bu metotla bizi cookiden kaldırıcak ve logout edicek*/
        }
    }

}


