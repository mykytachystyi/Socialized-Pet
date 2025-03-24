using MediatR;

namespace UseCases.Appeals.Messages.Commands.UpdateAppealMessage;

public record class UpdateAppealMessageCommand : IRequest<UpdateAppealMessageCommandResponse>
{
    public long UserId { get; set; }
    public long MessageId { get; set; }
    public required string Message { get; set; }
}
public record class UpdateAppealMessageCommandResponse(bool Success, string Message);
