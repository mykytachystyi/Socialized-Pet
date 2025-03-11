using MediatR;
using UseCases.Admins.Models;

namespace UseCases.Admins.Commands.Authentication;

public record class AuthenticationCommand : IRequest<AdminResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}