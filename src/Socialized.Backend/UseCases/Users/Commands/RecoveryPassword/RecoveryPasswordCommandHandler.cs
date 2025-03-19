using Core.Providers.Rand;
using Domain.Users;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Emails;

namespace UseCases.Users.Commands.RecoveryPassword;

public class RecoveryPasswordCommandHandler (
    IRepository<User> userRepository,
    IRandomizer randomizer,
    IEmailMessanger emailMessanger,
    ILogger logger) : IRequestHandler<RecoveryPasswordCommand, RecoveryPasswordResponse>
{
    public async Task<RecoveryPasswordResponse> Handle(RecoveryPasswordCommand request, 
        CancellationToken cancellationToken)
    {
        logger.Information($"Початок відновлення паролю, email={request.UserEmail}.");
        var user = await userRepository.FirstOrDefaultAsync(u => u.Email == request.UserEmail && u.Activate && !u.IsDeleted);

        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по email для активації аккаунту.");
        }
        user.RecoveryCode = randomizer.CreateCode(6);
        userRepository.Update(user);
        emailMessanger.SendRecoveryEmail(user.Email, request.Culture, (int)user.RecoveryCode);
        logger.Information($"Пароль був востановлений для користувача, id={user.Id}.");
        return new RecoveryPasswordResponse(true, "Пароль був востановлений.");
    }
}