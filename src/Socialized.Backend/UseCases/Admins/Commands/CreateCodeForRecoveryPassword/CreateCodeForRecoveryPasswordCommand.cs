using MediatR;

namespace UseCases.Admins.Commands.CreateCodeForRecoveryPassword;

public class CreateCodeForRecoveryPasswordCommand : IRequest<CreateCodeForRecoveryPasswordResponse>
{
    public string AdminEmail { get; set; }
}
public record CreateCodeForRecoveryPasswordResponse(bool Success, string Message);
