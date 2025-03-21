using AutoMapper;
using Domain.Appeals;
using Domain.Users;
using UseCases.Appeals.Files.Models;
using UseCases.Appeals.Messages.Models;
using UseCases.Appeals.Models;
using UseCases.Users.DefaultAdmin.Models;
using UseCases.Users.DefaultUser.Models;

namespace UseCases.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {

            CreateMap<User, UserResponse>();
            CreateMap<UserResponse, User>();

            CreateMap<User, LoginTokenResponse>();
            CreateMap<LoginTokenResponse, User>();

            CreateMap<Appeal, AppealResponse>();
            CreateMap<AppealResponse, Appeal>();

            CreateMap<AppealMessage, AppealMessageResponse>();
            CreateMap<AppealMessageResponse, AppealMessage>();

            CreateMap<AppealFileResponse, AppealFile>();
            CreateMap<AppealFile, AppealFileResponse>();
        }
    }
}