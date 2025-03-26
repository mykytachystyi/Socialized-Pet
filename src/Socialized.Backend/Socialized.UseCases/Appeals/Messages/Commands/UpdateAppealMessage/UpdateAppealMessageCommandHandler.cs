using MediatR;
using Serilog;
using UseCases.Exceptions;
using Domain.Appeals;
using Infrastructure.Repositories;

namespace UseCases.Appeals.Messages.Commands.UpdateAppealMessage;

public class UpdateAppealMessageCommandHandler(
    IRepository<AppealMessage> appealMessageRepository,
    ILogger logger
    ) : IRequestHandler<UpdateAppealMessageCommand, UpdateAppealMessageCommandResponse>
{
    public async Task<UpdateAppealMessageCommandResponse> Handle(UpdateAppealMessageCommand command,
        CancellationToken cancellationToken)
    {
        logger.Information($"Оновлення повідомлення, id={command.MessageId}...");

        var message = await appealMessageRepository.FirstOrDefaultAsync(m => m.Id == command.MessageId);
        if (message == null)
        {
            throw new NotFoundException("Повідомлення не було визначенно сервером по id.");
        }
        message.Message = command.Message;
        appealMessageRepository.Update(message);
        logger.Information($"Повідомлення було оновленно, id={message.Id}..");
        return new UpdateAppealMessageCommandResponse(true, "Повідомлення було оновленно.");
    }
}