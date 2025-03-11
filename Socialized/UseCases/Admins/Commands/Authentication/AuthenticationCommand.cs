using MediatR;
using UseCases.Response;

namespace UseCases.Admins.Commands.Authentication;

public record class AuthenticationCommand : IRequest<AdminResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}