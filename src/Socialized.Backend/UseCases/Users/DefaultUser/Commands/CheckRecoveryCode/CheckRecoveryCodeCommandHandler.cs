using Core.Providers.Rand;
using Domain.Users;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Users.DefaultUser.Commands.CheckRecoveryCode;

public class CheckRecoveryCodeCommandHandler(
    ILogger logger,
    IRepository<User> userRepository,
    IRandomizer randomizer
    ) : IRequestHandler<CheckRecoveryCodeCommand, CheckRecoveryCodeResponse>
{
    public async Task<CheckRecoveryCodeResponse> Handle(CheckRecoveryCodeCommand request,
        CancellationToken cancellationToken)
    {
        logger.Information($"Початок перевірки коду для зміни паролю, email={request.Email}.");
        var user = await userRepository.FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted);
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