using MediatR;

namespace UseCases.Users.DefaultUser.Commands.CreateUser;

public record class CreateUserCommand
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
}

public record class CreateUserWithRoleCommand(int Role, string Culture) : CreateUserCommand, IRequest<CreateUserResponse>;

public record class CreateUserResponse(bool Success, string Message);