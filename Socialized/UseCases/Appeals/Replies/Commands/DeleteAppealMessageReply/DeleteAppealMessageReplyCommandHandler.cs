using Domain.Appeals.Repositories;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Appeals.Replies.Commands.DeleteAppealMessageReply;

public class DeleteAppealMessageReplyCommandHandler(
    IAppealMessageReplyRepository replyRepository,
    ILogger logger) : IRequestHandler<DeleteAppealMessageReplyCommand,
    DeleteAppealMessageReplyResponse>
{
    public async Task<DeleteAppealMessageReplyResponse> Handle(DeleteAppealMessageReplyCommand request, 
        CancellationToken cancellationToken)
    {
        logger.Information("Початок видалення відповіді на повідомлення.");
        var reply = replyRepository.Get(request.ReplyId);
        if (reply == null)
        {
            throw new NotFoundException("Відповідь не була знайдена по id.");
        }
        reply.IsDeleted = true;
        replyRepository.Update(reply);
        logger.Information($"Відповідь була видаленна, id={reply.Id}.");
        return new DeleteAppealMessageReplyResponse(true, "Відповідь була видаленна.");
    }
}
