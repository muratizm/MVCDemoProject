using internshipProject1.Entities;
using System.Security.Claims;
using static internshipProject1.Shared.UserSharedDtos;

namespace internshipProject1.Shared
{
    public class UserSharedFunctions
    {
        public static UserInfoDto ProfileInfoLoader(ClaimsPrincipal User, DatabaseContext _databaseContext)
        {
            Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

            UserInfoDto userInfo = new UserInfoDto();

            userInfo.FullName = user.FullName;
            userInfo.ProfileImagePath = user.ProfileImageFileName;
            return userInfo;
        }
    }
}
