using MediatR;

namespace UseCases.Users.Commands.CreateUser;

public record class CreateUserCommand : IRequest<CreateUserResponse>
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    public required string CountryName { get; set; }
    public int TimeZone { get; set; }
    public required string Culture { get; set; }
}
public record class CreateUserResponse(bool Success, string Message);