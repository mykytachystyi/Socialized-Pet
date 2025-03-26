using MediatR;

namespace UseCases.Users.DefaultUser.Commands.RecoveryPassword;

public class RecoveryPasswordCommand : IRequest<RecoveryPasswordResponse>
{
    public required string UserEmail { get; set; }
    public required string Culture { get; set; }
}
public record class RecoveryPasswordResponse(bool Success, string Message);