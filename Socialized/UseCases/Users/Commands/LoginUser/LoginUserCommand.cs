using MediatR;
using UseCases.Users.Models;

namespace UseCases.Users.Commands.LoginUser;

public record class LoginUserCommand : IRequest<UserResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
