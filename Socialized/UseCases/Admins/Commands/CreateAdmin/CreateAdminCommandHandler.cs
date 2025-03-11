using Core;
using Serilog;
using System.Web;
using Domain.Admins;
using UseCases.Exceptions;
using MediatR;
using AutoMapper;
using UseCases.Admins.Models;
using UseCases.Admins.Emails;

namespace UseCases.Admins.Commands.CreateAdmin;

public class CreateAdminCommandHandler(
    IAdminRepository adminRepository,
    ProfileCondition profileCondition,
    AdminEmailManager adminEmailManager,
    ILogger logger,
    IMapper mapper
    ) : IRequestHandler<CreateAdminCommand, AdminResponse>
{
    public async Task<AdminResponse> Handle(CreateAdminCommand command, CancellationToken cancellationToken)
    {
        if (adminRepository.GetByEmail(command.Email) != null)
        {
            throw new NotFoundException($"Admin with email={command.Email} is already exist.");
        }
        var admin = new Admin
        {
            Email = command.Email,
            FirstName = HttpUtility.UrlDecode(command.FirstName),
            LastName = HttpUtility.UrlDecode(command.LastName),
            Password = profileCondition.HashPassword(command.Password),
            Role = "default",
            TokenForStart = profileCondition.CreateHash(10),
            CreatedAt = DateTime.UtcNow,
            LastLoginAt = DateTime.UtcNow
        };
        adminRepository.Create(admin);
        adminEmailManager.SetupPassword(admin.TokenForStart, admin.Email);
        logger.Information($"Був створений новий адмін, id={admin.Id}.");
        return mapper.Map<AdminResponse>(admin);
    }
}