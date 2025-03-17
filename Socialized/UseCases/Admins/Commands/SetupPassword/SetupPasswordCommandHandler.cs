using Core.Providers;
using Domain.Admins;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Admins.Commands.SetupPassword;

public class SetupPasswordCommandHandler(
    ILogger logger,
    IAdminRepository adminRepository,
    IEncryptionProvider encryptionProvider) : IRequestHandler<SetupPasswordCommand, SetupPasswordResult>
{
    public async Task<SetupPasswordResult> Handle(SetupPasswordCommand request, CancellationToken cancellationToken)
    {
        var admin = adminRepository.GetByPasswordToken(request.Token);
        if (admin == null)
        {
            throw new NotFoundException("Не було знайдено адміна по токену для зміни паролю.");
        }
        var newHashedPassword = encryptionProvider.HashPassword(request.Password);
        admin.HashedPassword = newHashedPassword.Hash;
        admin.HashedSalt = newHashedPassword.Salt;
        admin.TokenForStart = "";
        adminRepository.Update(admin);
        logger.Information($"Був налаштован пароль для адміна id={admin.Id}.");
        return new SetupPasswordResult(true, "Пароль було успішно змінено.");
    }
}