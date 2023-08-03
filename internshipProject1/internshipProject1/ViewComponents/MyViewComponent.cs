using internshipProject1.Entities;
using internshipProject1.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace internshipProject1.ViewComponents
{
    public class MyViewComponent : ViewComponent
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;

        public IViewComponentResult Invoke()
        {

            UserSharedDtos.UserInfoDto userInfo = UserSharedFunctions.ProfileInfoLoader((ClaimsPrincipal)User, _databaseContext);
            ViewData["FullName"] = userInfo.FullName;
            ViewData["ProfileImage"] = userInfo.ProfileImagePath;



            return View(); // View() metodu, View Component'ın geri dönüş yapacağı View sayfasını belirtir ve verileri gönderir.
        }

    }
}
