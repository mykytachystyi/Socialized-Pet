using Domain.Users;
using MediatR;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Emails;

namespace UseCases.Users.Commands.RegistrationEmail;

public class RegistrationEmailCommandHandler (
    IUserRepository userRepository,
    IEmailMessanger emailMessanger,
    ILogger logger) : IRequestHandler<RegistrationEmailCommand, RegistrationEmailResponse>
{
    public async Task<RegistrationEmailResponse> Handle(RegistrationEmailCommand request, CancellationToken cancellationToken)
    {
        logger.Information($"Початок відправлення листа на підтвердження реєстрації користувача, email={request.UserEmail}.");
        var user = userRepository.GetByEmail(request.UserEmail);
        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по email для активації аккаунту.");
        }
        emailMessanger.SendConfirmEmail(user.Email, request.Culture, user.HashForActivate);
        logger.Information($"Відправлен лист на підтверждення реєстрації користувача, id={user.Id}.");
        return new RegistrationEmailResponse(true, $"Відправлен лист на підтверждення реєстрації користувача, id={user.Id}.");
    }
}