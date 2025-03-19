using Core.Providers.Hmac;
using Domain.Admins;
using Infrastructure.Repositories;
using MediatR;
using Serilog;

namespace UseCases.Admins.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    IRepository<Admin> adminRepository,
    IEncryptionProvider encryptionProvider,
    ILogger logger) : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
{
    public async Task<ChangePasswordResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var admin = await adminRepository.FirstOrDefaultAsync(a => a.RecoveryCode == request.RecoveryCode && !a.IsDeleted);
        if (admin == null)
        {
            throw new ArgumentNullException("Сервер не визначив адміна по коду. Неправильний код.");
        }
        var newHashedPassword = encryptionProvider.HashPassword(request.Password);
        admin.HashedPassword = newHashedPassword.Hash;
        admin.HashedSalt = newHashedPassword.Salt;
        admin.RecoveryCode = null;
        adminRepository.Update(admin);
        logger.Information($"Був змінений пароль у адміна, id={admin.Id}.");
        return new ChangePasswordResponse(true, "Пароль адміну був зміненний.");
    }
}
