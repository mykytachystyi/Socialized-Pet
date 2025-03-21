using Domain.Users;
using MediatR;

namespace UseCases.Users.DefaultUser.Commands.LoginUser;

public record class LoginUserCommand
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
public record class LoginUserWithRoleCommand : LoginUserCommand, IRequest<User>
{
    public required int Role { get; set; }
}