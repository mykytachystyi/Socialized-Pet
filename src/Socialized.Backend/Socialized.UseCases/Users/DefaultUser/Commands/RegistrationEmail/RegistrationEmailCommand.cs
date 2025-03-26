using MediatR;

namespace UseCases.Users.DefaultUser.Commands.RegistrationEmail;

public class RegistrationEmailCommand : IRequest<RegistrationEmailResponse>
{
    public required string UserEmail { get; set; }
    public required string Culture { get; set; }
}
public record RegistrationEmailResponse(bool Success, string Message);