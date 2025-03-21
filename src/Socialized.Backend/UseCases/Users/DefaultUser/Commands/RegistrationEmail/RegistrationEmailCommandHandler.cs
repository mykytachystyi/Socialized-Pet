using Domain.Enums;
using Domain.Users;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.DefaultUser.Emails;

namespace UseCases.Users.DefaultUser.Commands.RegistrationEmail;

public class RegistrationEmailCommandHandler(
    IRepository<User> userRepository,
    IEmailMessanger emailMessanger,
    ILogger logger) : IRequestHandler<RegistrationEmailCommand, RegistrationEmailResponse>
{
    public async Task<RegistrationEmailResponse> Handle(RegistrationEmailCommand request, CancellationToken cancellationToken)
    {
        logger.Information($"Початок відправлення листа на підтвердження реєстрації користувача, email={request.UserEmail}.");
        
        var user = await userRepository.FirstOrDefaultAsync(
            u => u.Email == request.UserEmail 
                && !u.IsDeleted 
                && u.Role == (int)IdentityRole.DefaultUser);

        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по email для активації аккаунту.");
        }
        emailMessanger.SendConfirmEmail(user.Email, request.Culture, user.HashForActivate);
        logger.Information($"Відправлен лист на підтверждення реєстрації користувача, id={user.Id}.");
        return new RegistrationEmailResponse(true, $"Відправлен лист на підтверждення реєстрації користувача, id={user.Id}.");
    }
}