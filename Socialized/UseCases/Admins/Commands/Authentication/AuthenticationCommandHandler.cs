using Core;
using Domain.Admins;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Admins.Commands.Authentication;

public class AuthenticationCommandHandler(IAdminRepository adminRepository,
    ILogger logger,
    ProfileCondition profileCondition
    ) : IRequestHandler<AuthenticationCommand, Admin>
{
    public async Task<Admin> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
    {
        var admin = adminRepository.GetByEmail(request.Email);
        if (admin == null)
        {
            throw new NotFoundException("Не було знайдено адміна по email-адресі.");
        }
        if (!profileCondition.VerifyHashedPassword(admin.Password, request.Password))
        {
            throw new ValidationException("Невірний пароль.");
        }
        logger.Information($"Був аутентифікований адмін id={admin.Id}.");
        return admin;
    }
}