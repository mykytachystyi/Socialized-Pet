using Domain.Admins;
using MediatR;

namespace UseCases.Admins.Commands.Authentication;

public record class AuthenticationCommand : IRequest<Admin>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}