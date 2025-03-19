using MediatR;
using UseCases.Admins.Models;

namespace UseCases.Admins.Commands.CreateAdmin;

public class CreateAdminCommand : IRequest<AdminResponse>
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
}