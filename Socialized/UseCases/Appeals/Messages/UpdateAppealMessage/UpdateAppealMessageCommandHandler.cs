using MediatR;
using Serilog;
using UseCases.Exceptions;
using Domain.Appeals.Repositories;

namespace UseCases.Appeals.Messages.UpdateAppealMessage;

public class UpdateAppealMessageCommandHandler(
    IAppealMessageRepository appealMessageRepository,
    ILogger logger
    ) : IRequestHandler<UpdateAppealMessageCommand, UpdateAppealMessageCommandResponse>
{
    public async Task<UpdateAppealMessageCommandResponse> Handle(UpdateAppealMessageCommand request, 
        CancellationToken cancellationToken)
    {
        var message = appealMessageRepository.GetBy(request.MessageId);
        if (message == null)
        {
            throw new NotFoundException("Повідомлення не було визначенно сервером по id.");
        }
        message.Message = request.Message;
        appealMessageRepository.Update(message);
        logger.Information($"Повідомлення було оновленно, id={message.Id}..");
        return new UpdateAppealMessageCommandResponse(true, "Повідомлення було оновленно.");
    }
}
