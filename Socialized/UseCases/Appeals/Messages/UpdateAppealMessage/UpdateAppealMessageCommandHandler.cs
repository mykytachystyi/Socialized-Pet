using MediatR;
using Serilog;
using UseCases.Exceptions;
using Domain.Appeals;
using Infrastructure.Repositories;

namespace UseCases.Appeals.Messages.UpdateAppealMessage;

public class UpdateAppealMessageCommandHandler(
    IRepository<AppealMessage> appealMessageRepository,
    ILogger logger
    ) : IRequestHandler<UpdateAppealMessageCommand, UpdateAppealMessageCommandResponse>
{
    public async Task<UpdateAppealMessageCommandResponse> Handle(UpdateAppealMessageCommand request, 
        CancellationToken cancellationToken)
    {
        var message = await appealMessageRepository.FirstOrDefaultAsync(m => m.Id == request.MessageId);
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