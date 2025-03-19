using MediatR;

namespace UseCases.Appeals.Replies.Commands.UpdateAppealMessageReply;

public class UpdateAppealMessageReplyCommand : IRequest<UpdateAppealMessageReplyResponse>
{
    public long ReplyId { get; set; }
    public required string Reply { get; set; }
}
public record UpdateAppealMessageReplyResponse(bool Success, string Message);
