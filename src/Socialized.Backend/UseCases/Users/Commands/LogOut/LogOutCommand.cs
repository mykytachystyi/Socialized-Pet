using MediatR;

namespace UseCases.Users.Commands.LogOut;

public record class LogOutCommand : IRequest<LogOutResponse>
{
    public string UserToken { get; set; }
}
public record class LogOutResponse(bool Success, string Message);