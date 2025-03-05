using Domain.Users;
using AutoMapper;
using UseCases.Response;

namespace WebApi.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponse>();

            CreateMap<UserResponse, User>();
        }
    }
}

