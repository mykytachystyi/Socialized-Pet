using MediatR;

namespace UseCases.Appeals.Replies.Commands.DeleteAppealMessageReply;

public class DeleteAppealMessageReplyCommand : IRequest<DeleteAppealMessageReplyResponse>
{
    public long ReplyId { get; set; }
}
public record class DeleteAppealMessageReplyResponse(bool Success, string Message);
