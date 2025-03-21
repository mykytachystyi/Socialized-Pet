using Domain.Users;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Users.DefaultUser.Commands.Activate;

public class ActivateCommandHandler(
    IRepository<User> userRepository,
    ILogger logger
    ) : IRequestHandler<ActivateCommand, ActivateResponse>
{
    public async Task<ActivateResponse> Handle(ActivateCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Початок активації аккаунту користувача за допомогою хешу.");
        var user = await userRepository.FirstOrDefaultAsync(
            u => u.HashForActivate == request.Hash && !u.Activate && !u.IsDeleted);
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
