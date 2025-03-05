using AutoMapper;
using Domain.Admins;
using UseCases.Response;

namespace WebApiCompose.Mapping
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            CreateMap<Admin, AdminResponse>();

            CreateMap<AdminResponse, Admin>();
        }
    }
}
