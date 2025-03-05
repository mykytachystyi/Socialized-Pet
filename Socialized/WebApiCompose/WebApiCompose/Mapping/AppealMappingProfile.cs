using AutoMapper;
using Domain.Admins;
using UseCases.Response.Appeals;

namespace WebApiCompose.Mapping
{
    public class AppealMappingProfile : Profile
    {
        public AppealMappingProfile()
        {
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
