using Domain.Users;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Users.Commands.Activate;

public class ActivateCommandHandler (
    IUserRepository userRepository,
    ILogger logger
    ) : IRequestHandler<ActivateCommand, ActivateResponse>
{
    public async Task<ActivateResponse> Handle(ActivateCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Початок активації аккаунту користувача за допомогою хешу.");
        var user = userRepository.GetByHash(request.Hash, false, false);
        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по хешу для активації аккаунту.");
        }
        user.Activate = true;
        userRepository.Update(user);
        logger.Information($"Користувач був активований завдяки хешу з пошти, id={user.Id}.");
        return new ActivateResponse(true, "Аккаунт користувача був активований.");
    }
}
