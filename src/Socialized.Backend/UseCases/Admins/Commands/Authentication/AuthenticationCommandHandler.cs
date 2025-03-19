using Core.Providers.Hmac;
using Domain.Admins;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Admins.Commands.Authentication;

public class AuthenticationCommandHandler(IRepository<Admin> adminRepository,
    ILogger logger,
    IEncryptionProvider encryptionProvider
    ) : IRequestHandler<AuthenticationCommand, Admin>
{
    public async Task<Admin> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
    {
        var admin = await adminRepository.FirstOrDefaultAsync(a => a.Email == request.Email && !a.IsDeleted);
        if (admin == null)
        {
            throw new NotFoundException("Не було знайдено адміна по email-адресі.");
        }
        if (!encryptionProvider.VerifyPasswordHash(request.Password,
            new SaltAndHash { Hash = admin.HashedPassword, Salt = admin.HashedSalt }))
        {
            throw new ValidationException("Невірний пароль.");
        }
        logger.Information($"Був аутентифікований адмін id={admin.Id}.");
        return admin;
    }
}