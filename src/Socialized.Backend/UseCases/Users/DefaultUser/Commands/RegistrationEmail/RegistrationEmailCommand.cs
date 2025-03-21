using MediatR;

namespace UseCases.Users.DefaultUser.Commands.RegistrationEmail;

public class RegistrationEmailCommand : IRequest<RegistrationEmailResponse>
{
    public string UserEmail { get; set; }
    public string Culture { get; set; }
}
public record RegistrationEmailResponse(bool Success, string Message);