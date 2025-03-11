using MediatR;

namespace UseCases.Admins.Commands.Delete;

public record class DeleteAdminCommand : IRequest<DeleteAdminResponse>
{
    public long AdminId { get; set; }
}
public record DeleteAdminResponse(bool Success, string Message);

