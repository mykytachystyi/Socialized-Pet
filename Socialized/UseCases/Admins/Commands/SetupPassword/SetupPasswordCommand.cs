using MediatR;

namespace UseCases.Admins.Commands.SetupPassword;

public class SetupPasswordCommand : IRequest<SetupPasswordResponse>
{
    public required string Token { get; set; }
    public required string Password { get; set; }
}
public record class SetupPasswordResponse(bool Success, string Message);