using Core.Providers.TextEncrypt;
using Domain.Users;
using MediatR;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Emails;

namespace UseCases.Users.Commands.RecoveryPassword;

public class RecoveryPasswordCommandHandler (
    IUserRepository userRepository,
    TextEncryptionProvider profileCondition,
    IEmailMessanger emailMessanger,
    ILogger logger) : IRequestHandler<RecoveryPasswordCommand, RecoveryPasswordResponse>
{
    public async Task<RecoveryPasswordResponse> Handle(RecoveryPasswordCommand request, 
        CancellationToken cancellationToken)
    {
        logger.Information($"Початок відновлення паролю, email={request.UserEmail}.");
        var user = userRepository.GetByEmail(request.UserEmail, false, true);

        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по email для активації аккаунту.");
        }
        user.RecoveryCode = profileCondition.CreateCode(6);
        userRepository.Update(user);
        emailMessanger.SendRecoveryEmail(user.Email, request.Culture, (int)user.RecoveryCode);
        logger.Information($"Пароль був востановлений для користувача, id={user.Id}.");
        return new RecoveryPasswordResponse(true, "Пароль був востановлений.");
    }
}