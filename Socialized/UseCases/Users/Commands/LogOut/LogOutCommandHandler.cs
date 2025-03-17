using Core.Providers.Rand;
using Domain.Users;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Users.Commands.LogOut;

public class LogOutCommandHandler (
    IRepository<User> userRepository,
    IRandomizer randomizer,
    ILogger logger) : IRequestHandler<LogOutCommand, LogOutResponse>
{
    public async Task<LogOutResponse> Handle(LogOutCommand request, CancellationToken cancellationToken)
    {
        logger.Information($"Початок виходу(logout) користувача.");

        var user = await userRepository.FirstOrDefaultAsync(u => u.TokenForUse == request.UserToken && !u.IsDeleted);
        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по токен для активації аккаунту.");
        }
        user.TokenForUse = randomizer.CreateHash(40);
        userRepository.Update(user);
        logger.Information($"Користувач вийшов з сервісу, id={user.Id}.");

        return new LogOutResponse(true, "Користувач вийшов з сервісу.");
    }
}
