using MediatR;

namespace UseCases.Users.DefaultUser.Commands.ChangeOldPassword;

public class ChangeOldPasswordCommand
{
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}
public class ChangeOldPasswordWithUserCommand : ChangeOldPasswordCommand, IRequest<ChangeOldPasswordResponse>
{
    public long UserId { get; set; }
}
public record class ChangeOldPasswordResponse(bool Success, string Message);