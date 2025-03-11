using Core;
using Domain.Admins;
using MediatR;
using Serilog;

namespace UseCases.Admins.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    IAdminRepository adminRepository,
    ProfileCondition profileCondition,
    ILogger logger) : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
{
    public async Task<ChangePasswordResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var admin = adminRepository.GetByRecoveryCode(request.RecoveryCode);
        if (admin == null)
        {
            throw new ArgumentNullException("Сервер не визначив адміна по коду. Неправильний код.");
        }
        admin.Password = profileCondition.HashPassword(request.Password);
        admin.RecoveryCode = null;
        adminRepository.Update(admin);
        logger.Information($"Був змінений пароль у адміна, id={admin.Id}.");
        return new ChangePasswordResponse(true, "Пароль адміну був зміненний.");
    }
}
