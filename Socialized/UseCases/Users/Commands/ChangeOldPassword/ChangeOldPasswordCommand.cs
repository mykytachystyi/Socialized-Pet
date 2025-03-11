using MediatR;

namespace UseCases.Users.Commands.ChangeOldPassword;

public class ChangeOldPasswordCommand : IRequest<ChangeOldPasswordResponse>
{
    public required string UserToken { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}
public record class ChangeOldPasswordResponse(bool Success, string Message);