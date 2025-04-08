using Mapster;
using Domain.Appeals;
using Domain.Users;
using UseCases.Appeals.Files.Models;
using UseCases.Appeals.Messages.Models;
using UseCases.Appeals.Models;
using UseCases.Users.DefaultAdmin.Models;
using UseCases.Users.DefaultUser.Models;

namespace UseCases.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, UserResponse>();
            config.NewConfig<User, UserResponse>();

            config.NewConfig<User, LoginTokenResponse>();
            config.NewConfig<LoginTokenResponse, UserResponse>();

            config.NewConfig<User, LoginTokenResponse>();
            config.NewConfig<LoginTokenResponse, UserResponse>();

            config.NewConfig<Appeal, AppealResponse>();
            config.NewConfig<AppealResponse, Appeal>();

            config.NewConfig<AppealMessage, AppealMessageResponse>();
            config.NewConfig<AppealMessageResponse, AppealMessage>();

            config.NewConfig<AppealFileResponse, AppealFile>();
            config.NewConfig<AppealFile, AppealFileResponse>();
        }
    }
}