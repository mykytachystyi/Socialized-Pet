using MediatR;
using UseCases.Base;
using UseCases.Appeals.Messages.Models;

namespace UseCases.Appeals.Messages.CreateAppealMessage;

public class CreateAppealMessageCommand : IRequest<AppealMessageResponse>
{
    public long AppealId { get; set; }
    public required string Message { get; set; }
    public required string UserToken { get; set; }
    public ICollection<FileDto>? Files { get; set; }
}
