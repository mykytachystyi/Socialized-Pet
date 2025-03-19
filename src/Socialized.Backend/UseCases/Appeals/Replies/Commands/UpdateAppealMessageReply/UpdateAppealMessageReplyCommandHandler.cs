using Domain.Appeals;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Appeals.Replies.Commands.UpdateAppealMessageReply;

public class UpdateAppealMessageReplyCommandHandler (
    IRepository<AppealMessageReply> replyRepository,
    ILogger logger 
    ) : IRequestHandler<UpdateAppealMessageReplyCommand, 
    UpdateAppealMessageReplyResponse>
{
    public async Task<UpdateAppealMessageReplyResponse> Handle(UpdateAppealMessageReplyCommand request, 
        CancellationToken cancellationToken)
    {
        logger.Information("Початок оновлення відповіді на повідомлення.");

        var reply = await replyRepository.FirstOrDefaultAsync(r => r.Id == request.ReplyId);
        if (reply == null)
        {
            throw new NotFoundException("Відповідь не була знайдена по id.");
        }
        reply.Reply = request.Reply;
        replyRepository.Update(reply);

        logger.Information($"Відповідь була оновленна, id={reply.Id}.");
        return new UpdateAppealMessageReplyResponse(true, "Відповідь була оновленна.");
    }
}
