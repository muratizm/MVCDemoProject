using AutoMapper;
using internshipProject1.Entities;
using internshipProject1.Models;
using internshipProject1.Shared;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;

namespace internshipProject1.Controllers
{
    public class UserController : Controller
    {

        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(DatabaseContext databaseContext, IMapper mapper, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            UserSharedDtos.UserInfoDto userInfo = UserSharedFunctions.ProfileInfoLoader(User, _databaseContext);
            ViewData["FullName"] = userInfo.FullName;
            ViewData["ProfileImage"] = userInfo.ProfileImagePath;

            List<UserModel> users = _databaseContext.Users.ToList().Select(x => _mapper.Map<UserModel>(x)).ToList();
            

            _databaseContext.Users.Select(x => new UserModel { Id = x.Id, FullName = x.FullName }).ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists. ");
                    return View(model);
                }

                User user = _mapper.Map<User>(model);
                _databaseContext.Users.Add(user);
                user.Password = MD5HashedString(model.Password);
                _databaseContext.SaveChanges();


                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Edit(Guid id)
        {
            User user = _databaseContext.Users.Find(id);
            EditUserModel mode = _mapper.Map<EditUserModel>(user);

            return View();
        }
        [HttpPost]

        public IActionResult Edit(Guid id, EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower() && x.Id != id))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists. ");
                    return View(model);
                }

                User user = _databaseContext.Users.Find(id);
                _mapper.Map(model,user);
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]

        public IActionResult Delete(Guid id)
        {
           
                User user = _databaseContext.Users.Find(id);
                if(user != null)
            {
                _databaseContext.Users.Remove(user);
                _databaseContext.SaveChanges();
            }

                return RedirectToAction(nameof(Index));
            }
        private string MD5HashedString(string s)
        {
            string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
            string salted = s + md5Salt;
            string hashed = salted.MD5();
            return hashed;
        }

    }
    

}

