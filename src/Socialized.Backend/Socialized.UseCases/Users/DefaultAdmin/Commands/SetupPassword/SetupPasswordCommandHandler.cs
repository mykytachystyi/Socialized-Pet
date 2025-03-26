using Core.Providers.Hmac;
using Domain.Users;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Users.DefaultAdmin.Commands.SetupPassword;

public class SetupPasswordCommandHandler(
    ILogger logger,
    IRepository<User> userRepository,
    IEncryptionProvider encryptionProvider) : IRequestHandler<SetupPasswordWithAdminCommand, SetupPasswordResponse>
{
    public async Task<SetupPasswordResponse> Handle(SetupPasswordWithAdminCommand request, CancellationToken cancellationToken)
    {
        logger.Information($"Початок налаштування паролю для адміна, id={request.AdminId}.");

        var admin = await userRepository.FirstOrDefaultAsync(a => a.Id == request.AdminId && !a.IsDeleted);
        if (admin == null)
        {
            throw new NotFoundException("Не було знайдено адміна по токену для зміни паролю.");
        }
        var newHashedPassword = encryptionProvider.HashPassword(request.Password);
        admin.HashedPassword = newHashedPassword.Hash;
        admin.HashedSalt = newHashedPassword.Salt;
        userRepository.Update(admin);
        
        logger.Information($"Був налаштован пароль для адміна id={admin.Id}.");
        return new SetupPasswordResponse(true, "Пароль було успішно змінено.");
    }
}