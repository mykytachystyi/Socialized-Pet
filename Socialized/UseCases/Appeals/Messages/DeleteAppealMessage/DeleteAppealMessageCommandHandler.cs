using MediatR;
using Serilog;
using UseCases.Exceptions;
using Domain.Appeals.Repositories;

namespace UseCases.Appeals.Messages.DeleteAppealMessage;

public class DeleteAppealMessageCommandHandler(
    IAppealMessageRepository appealMessageRepository,
    ILogger logger
    ) : IRequestHandler<DeleteAppealMessageCommand, DeleteAppealMessageResponse>
{
    public async Task<DeleteAppealMessageResponse> Handle(
        DeleteAppealMessageCommand request, 
        CancellationToken cancellationToken)
    {
        var message = appealMessageRepository.GetBy(request.MessageId);
        if (message == null)
        {
            throw new NotFoundException("Повідомлення не було визначенно сервером по id.");
        }
        message.IsDeleted = true;
        appealMessageRepository.Update(message);
        logger.Information($"Повідомлення було видаленно, id={message.Id}.");
        return new DeleteAppealMessageResponse(true, $"Повідомлення було видаленно, id={message.Id}.");
    }
}
