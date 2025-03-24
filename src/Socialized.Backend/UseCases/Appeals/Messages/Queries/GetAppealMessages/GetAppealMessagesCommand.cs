using MediatR;
using UseCases.Appeals.Messages.Models;

namespace UseCases.Appeals.Messages.Queries.GetAppealMessages;

public class GetAppealMessagesCommand : IRequest<IEnumerable<AppealMessageResponse>>
{
    public long AppealId { get; set; }
    public int Since { get; set; }
    public int Count { get; set; }
}