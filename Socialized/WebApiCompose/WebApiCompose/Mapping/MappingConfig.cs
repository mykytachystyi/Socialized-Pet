using AutoMapper;
using Domain.Admins;
using Domain.Users;
using UseCases.Response;
using UseCases.Response.Appeals;

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
