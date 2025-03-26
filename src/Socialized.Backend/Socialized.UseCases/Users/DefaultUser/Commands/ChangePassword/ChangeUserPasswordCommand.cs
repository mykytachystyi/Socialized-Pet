using MediatR;

namespace UseCases.Users.DefaultUser.Commands.ChangePassword;

public record class ChangeUserPasswordCommand : IRequest<ChangeUserPasswordResponse>
{
    public required string RecoveryToken { get; set; }
    public required string UserPassword { get; set; }
    public required string UserConfirmPassword { get; set; }
}
public record class ChangeUserPasswordResponse(bool Success, string Message);
