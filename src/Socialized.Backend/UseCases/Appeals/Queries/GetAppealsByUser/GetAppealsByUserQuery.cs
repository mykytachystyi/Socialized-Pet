using MediatR;
using UseCases.Appeals.Models;

namespace UseCases.Appeals.Queries.GetAppealsByUser;

public class GetAppealsByUserQuery : IRequest<IEnumerable<AppealResponse>>
{
    public long UserId { get; set; }
    public int Since { get; set; }
    public int Count { get; set; }
}
