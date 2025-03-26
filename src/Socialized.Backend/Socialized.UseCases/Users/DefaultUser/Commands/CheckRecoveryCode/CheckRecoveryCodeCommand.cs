using MediatR;

namespace UseCases.Users.DefaultUser.Commands.CheckRecoveryCode
{
    public record class CheckRecoveryCodeCommand : IRequest<CheckRecoveryCodeResponse>
    {
        public required string Email { get; set; }
        public int RecoveryCode { get; set; }
    }
}
public record class CheckRecoveryCodeResponse(string RecoveryToken);
