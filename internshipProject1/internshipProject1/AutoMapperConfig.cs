using AutoMapper;
using internshipProject1.Entities;
using internshipProject1.Models;

namespace internshipProject1
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User,UserModel>().ReverseMap();
            CreateMap<User, CreateUserModel>().ReverseMap();
            CreateMap<User, EditUserModel>().ReverseMap();


        }
    }
}
