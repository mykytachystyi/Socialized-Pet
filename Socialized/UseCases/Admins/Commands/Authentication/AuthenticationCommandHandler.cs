using AutoMapper;
using Core;
using Domain.Admins;
using MediatR;
using Serilog;
using UseCases.Exceptions;
using UseCases.Response;

namespace UseCases.Admins.Commands.Authentication;

public class AuthenticationCommandHandler(IAdminRepository adminRepository,
    ILogger logger,
    ProfileCondition profileCondition,
    IMapper mapper
    ) : IRequestHandler<AuthenticationCommand, AdminResponse>
{
    public async Task<AdminResponse> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
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
        return mapper.Map<AdminResponse>(admin);
    }
}