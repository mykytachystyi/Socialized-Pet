using MediatR;

namespace UseCases.Users.Commands.RegistrationEmail;

public class RegistrationEmailCommand : IRequest<RegistrationEmailResponse>
{
    public string UserEmail { get; set; }
    public string Culture { get; set; }
}
public record RegistrationEmailResponse(bool Success, string Message);