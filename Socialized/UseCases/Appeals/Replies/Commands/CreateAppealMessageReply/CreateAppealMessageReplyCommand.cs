using MediatR;
using UseCases.Appeals.Replies.Models;

namespace UseCases.Appeals.Replies.Commands.CreateAppealMessageReply;

public record class CreateAppealMessageReplyCommand : IRequest<AppealReplyResponse>
{
    public long AppealMessageId { get; set; }
    public required string Reply { get; set; }
}
