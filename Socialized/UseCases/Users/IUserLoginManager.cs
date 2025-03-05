using UseCases.Users.Commands;
using UseCases.Response;

namespace UseCases.Users
{
    public interface IUserLoginManager
    {
        UserResponse Login(LoginUserCommand command);
        void LogOut(string userToken);
    }
}
