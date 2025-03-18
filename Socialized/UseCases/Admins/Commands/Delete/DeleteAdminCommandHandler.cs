using Serilog;
using Domain.Admins;
using MediatR;
using UseCases.Exceptions;
using Infrastructure.Repositories;

namespace UseCases.Admins.Commands.Delete;

public class DeleteAdminCommandHandler(
    IRepository<Admin> adminRepository,
    ILogger logger
    ) : IRequestHandler<DeleteAdminCommand, DeleteAdminResponse>
{
    public async Task<DeleteAdminResponse> Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
    {
        logger.Information($"Початок видалення адміна, id={request.AdminId}.");
        var admin = await adminRepository.GetByIdAsync(request.AdminId);
        if (admin == null)
        {
            throw new NotFoundException("Не було знайдено адміна по id.");
        }
        admin.IsDeleted = true;
        admin.DeletedAt = DateTime.UtcNow;
        adminRepository.Update(admin);
        logger.Information($"Адмін був видалений, id={admin.Id}.");
        return new DeleteAdminResponse(true, "Адмін був успішно видалений.");
    }
}
