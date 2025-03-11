using MediatR;

namespace UseCases.Admins.Commands.CreateCodeForeRecoveryPassword;

public class CreateCodeForRecoveryPasswordCommand : IRequest<CreateCodeForRecoveryPasswordResponse>
{
    public string AdminEmail { get; set; }
}
public record CreateCodeForRecoveryPasswordResponse(bool Success, string Message);
