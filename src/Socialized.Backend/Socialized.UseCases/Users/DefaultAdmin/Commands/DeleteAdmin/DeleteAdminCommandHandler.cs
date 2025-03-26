using Serilog;
using MediatR;
using UseCases.Exceptions;
using Infrastructure.Repositories;
using Domain.Users;
using Domain.Enums;

namespace UseCases.Users.DefaultAdmin.Commands.DeleteAdmin;

public class DeleteAdminCommandHandler(
    IRepository<User> userRepository,
    ILogger logger
    ) : IRequestHandler<DeleteAdminCommand, DeleteAdminResponse>
{
    public async Task<DeleteAdminResponse> Handle(DeleteAdminCommand command, CancellationToken cancellationToken)
    {
        logger.Information($"Початок видалення адміна, id={command.AdminId}.");

        var admin = await userRepository.FirstOrDefaultAsync(
            u => u.Id == command.AdminId
            && u.Id != command.OwnerId
            && !u.IsDeleted
            && u.Role == (int)IdentityRole.DefaultAdmin);
        if (admin == null)
        {
            throw new NotFoundException("Не було знайдено адміна по id.");
        }
        admin.IsDeleted = true;
        admin.DeletedAt = DateTime.UtcNow;
        userRepository.Update(admin);

        logger.Information($"Адмін був видалений, id={admin.Id}.");
        return new DeleteAdminResponse(true, "Адмін був успішно видалений.");
    }
}