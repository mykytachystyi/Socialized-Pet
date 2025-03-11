using MediatR;

namespace UseCases.Users.Commands.RecoveryPassword;

public class RecoveryPasswordCommand : IRequest<RecoveryPasswordResponse>
{
    public string UserEmail { get; set; }
    public string Culture { get; set; }
}
public record class RecoveryPasswordResponse(bool Success, string Message);