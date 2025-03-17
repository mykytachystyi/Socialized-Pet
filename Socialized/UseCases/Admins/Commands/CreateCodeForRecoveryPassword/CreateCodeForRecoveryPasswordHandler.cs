using MediatR;
using Serilog;
using Domain.Admins;
using UseCases.Exceptions;
using UseCases.Admins.Emails;
using Core.Providers.Rand;
using Infrastructure.Repositories;

namespace UseCases.Admins.Commands.CreateCodeForRecoveryPassword;

public class CreateCodeForRecoveryPasswordHandler(
    IRepository<Admin> adminRepository,
    IAdminEmailManager adminEmailManager,
    ILogger logger,
    IRandomizer randomizer
    ) : IRequestHandler<CreateCodeForRecoveryPasswordCommand, CreateCodeForRecoveryPasswordResponse>
{
    public async Task<CreateCodeForRecoveryPasswordResponse> Handle(
        CreateCodeForRecoveryPasswordCommand request, CancellationToken cancellationToken)
    {
        var admin = await adminRepository.FirstOrDefaultAsync(a => a.Email == request.AdminEmail && !a.IsDeleted);
        if (admin == null)
        {
            throw new NotFoundException("Не було знайдено адміна по email-адресі.");
        }
        admin.RecoveryCode = randomizer.CreateCode(6);
        adminRepository.Update(admin);
        adminEmailManager.RecoveryPassword(admin.RecoveryCode.Value, admin.Email);
        logger.Information($"Був створений новий код відновлення паролю адміна, id={admin.Id}.");
        return new CreateCodeForRecoveryPasswordResponse(true, "Був створений новий код відновлення паролю адміна.");
    }
}