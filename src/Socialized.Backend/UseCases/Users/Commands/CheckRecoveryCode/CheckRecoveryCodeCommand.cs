using MediatR;

namespace UseCases.Users.Commands.CheckRecoveryCode
{
    public record class CheckRecoveryCodeCommand : IRequest<CheckRecoveryCodeResponse>
    {
        public required string UserEmail { get; set; }
        public int RecoveryCode { get; set; }
    }
}
public record class CheckRecoveryCodeResponse(string RecoveryToken);   
