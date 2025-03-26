using MediatR;

namespace UseCases.Users.DefaultAdmin.Commands.SetupPassword;

public record class SetupPasswordWithAdminCommand : SetupPasswordCommand, IRequest<SetupPasswordResponse>
{
    public required long AdminId { get; set; }
}
public record class SetupPasswordCommand
{
    public required string Password { get; set; }
}
public record class SetupPasswordResponse(bool Success, string Message);