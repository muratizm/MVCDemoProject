using internshipProject1.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using internshipProject1.Shared;

namespace internshipProject1.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        public AdminController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }
        public IActionResult Index()
        {

            UserSharedDtos.UserInfoDto userInfo =  UserSharedFunctions.ProfileInfoLoader(User, _databaseContext);
            ViewData["FullName"] = userInfo.FullName;
            ViewData["ProfileImage"] = userInfo.ProfileImagePath;

            return View();
        }

    }

}
