using MediatR;

namespace UseCases.Admins.Commands.SetupPassword;

public class SetupPasswordCommand : IRequest<SetupPasswordResult>
{
    public required string Token { get; set; }
    public required string Password { get; set; }
}
public record class SetupPasswordResult(bool Success, string Message);