using MediatR;

namespace UseCases.Users.DefaultAdmin.Commands.DeleteAdmin;

public record class DeleteAdminCommand : DeleteAdminWithoutOwnerCommand, IRequest<DeleteAdminResponse>
{
    public long OwnerId { get; set; }
}
public record class DeleteAdminWithoutOwnerCommand
{
    public long AdminId { get; set; }
}
public record DeleteAdminResponse(bool Success, string Message);

