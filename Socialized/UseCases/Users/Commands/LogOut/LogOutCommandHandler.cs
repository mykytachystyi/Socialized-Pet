using Core;
using Domain.Users;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Users.Commands.LogOut;

public class LogOutCommandHandler (
    IUserRepository userRepository,
    ProfileCondition profileCondition,
    ILogger logger) : IRequestHandler<LogOutCommand, LogOutResponse>
{
    public async Task<LogOutResponse> Handle(LogOutCommand request, CancellationToken cancellationToken)
    {
        logger.Information($"Початок виходу(logout) користувача.");

        var user = userRepository.GetByUserTokenNotDeleted(request.UserToken);
        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по токен для активації аккаунту.");
        }
        user.TokenForUse = profileCondition.CreateHash(40);
        userRepository.Update(user);
        logger.Information($"Користувач вийшов з сервісу, id={user.Id}.");

        return new LogOutResponse(true, "Користувач вийшов з сервісу.");
    }
}
