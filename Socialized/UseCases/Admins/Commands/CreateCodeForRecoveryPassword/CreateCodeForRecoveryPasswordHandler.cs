using Core;
using MediatR;
using Serilog;
using Domain.Admins;
using UseCases.Exceptions;
using UseCases.Admins.Emails;

namespace UseCases.Admins.Commands.CreateCodeForRecoveryPassword;

public class CreateCodeForRecoveryPasswordHandler(
    IAdminRepository adminRepository,
    IAdminEmailManager adminEmailManager,
    ILogger logger,
    ProfileCondition profileCondition
    ) : IRequestHandler<CreateCodeForRecoveryPasswordCommand, CreateCodeForRecoveryPasswordResponse>
{
    public async Task<CreateCodeForRecoveryPasswordResponse> Handle(
        CreateCodeForRecoveryPasswordCommand request, CancellationToken cancellationToken)
    {
        var admin = adminRepository.GetByEmail(request.AdminEmail);
        if (admin == null)
        {
            throw new NotFoundException("Не було знайдено адміна по email-адресі.");
        }
        admin.RecoveryCode = profileCondition.CreateCode(6);
        adminRepository.Update(admin);
        adminEmailManager.RecoveryPassword(admin.RecoveryCode.Value, admin.Email);
        logger.Information($"Був створений новий код відновлення паролю адміна, id={admin.Id}.");
        return new CreateCodeForRecoveryPasswordResponse(true, "Був створений новий код відновлення паролю адміна.");
    }
}