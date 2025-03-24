using MediatR;
using UseCases.Appeals.Messages.Models;
using UseCases.Appeals.Files.Models;

namespace UseCases.Appeals.Messages.Commands.CreateAppealMessage;

public record class CreateAppealMessageWithUserCommand : CreateAppealMessageCommand, IRequest<AppealMessageResponse>
{
    public long UserId { get; set; }
}
public record class CreateAppealMessageCommand
{
    public long AppealId { get; set; }
    public required string Message { get; set; }
    public ICollection<FileDto>? Files { get; set; }
}