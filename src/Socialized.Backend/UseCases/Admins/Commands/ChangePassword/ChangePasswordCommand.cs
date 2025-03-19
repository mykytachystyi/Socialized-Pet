using MediatR;

namespace UseCases.Admins.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<ChangePasswordResponse>
    {
        public required int RecoveryCode { get; set; }
        public required string Password { get; set; }
    }
}
public record ChangePasswordResponse(bool Success, string Message);