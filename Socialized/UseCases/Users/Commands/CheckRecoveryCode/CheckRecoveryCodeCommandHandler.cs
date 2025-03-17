using Core.Providers;
using Core.Providers.Rand;
using Core.Providers.TextEncrypt;
using Domain.Users;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Users.Commands.CheckRecoveryCode;

public class CheckRecoveryCodeCommandHandler (
    ILogger logger,
    IUserRepository userRepository,
    IRandomizer randomizer
    ) : IRequestHandler<CheckRecoveryCodeCommand, CheckRecoveryCodeResponse>
{
    public async Task<CheckRecoveryCodeResponse> Handle(CheckRecoveryCodeCommand request, 
        CancellationToken cancellationToken)
    {
        logger.Information($"Початок перевірки коду для зміни паролю, email={request.UserEmail}.");
        var user = userRepository.GetByEmail(request.UserEmail);
        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по email для перевірки коду востановлення паролю.");
        }
        if (user.RecoveryCode != request.RecoveryCode)
        {
            throw new ValidationException("Код востановлення паролю не вірний.");
        }
        user.RecoveryToken = randomizer.CreateHash(40);
        user.RecoveryCode = -1;
        userRepository.Update(user);
        logger.Information($"Перевірен був код востановлення паролю користувача, id={user.Id}.");
        return new CheckRecoveryCodeResponse(user.RecoveryToken);
    }
}