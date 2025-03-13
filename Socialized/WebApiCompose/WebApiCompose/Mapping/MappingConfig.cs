using AutoMapper;
using Domain.Admins;
using Domain.Appeals;
using Domain.Users;
using UseCases.Admins.Models;
using UseCases.Appeals.Files.Models;
using UseCases.Appeals.Messages.Models;
using UseCases.Appeals.Models;
using UseCases.Appeals.Replies.Models;
using UseCases.Users.Models;

namespace WebApiCompose.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Admin, AdminResponse>();
            CreateMap<AdminResponse, Admin>();

            CreateMap<User, UserResponse>();
            CreateMap<UserResponse, User>();

            CreateMap<Appeal, AppealResponse>();
            CreateMap<AppealResponse, Appeal>();

            CreateMap<AppealMessage, AppealMessageResponse>();
            CreateMap<AppealMessageResponse, AppealMessage>();

            CreateMap<AppealReplyResponse, AppealMessageReply>();
            CreateMap<AppealMessageReply, AppealReplyResponse>();

            CreateMap<AppealFileResponse, AppealFile>();
            CreateMap<AppealFile, AppealFileResponse>();
        }
    }
}
