using MediatR;
using Serilog;
using Domain.Users;
using UseCases.Exceptions;

namespace UseCases.Users.Commands.Delete;

public class DeleteCommandHandler (
    ILogger logger,
    IUserRepository userRepository) : IRequestHandler<DeleteCommand, DeleteResponse>
{
    public async Task<DeleteResponse> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Початок видалення користувача по його токену.");
        var user = userRepository.GetByUserTokenNotDeleted(request.UserToken);
        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по його токену для видалення аккаунту.");
        }
        user.IsDeleted = true;
        user.TokenForUse = "";
        userRepository.Update(user);
        logger.Information($"Користувач був видалений, id={user.Id}.");
        return new DeleteResponse(true, $"Користувач був видалений, id={user.Id}.");
    }
}