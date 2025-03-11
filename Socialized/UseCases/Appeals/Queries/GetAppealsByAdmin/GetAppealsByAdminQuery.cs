using MediatR;
using UseCases.Appeals.Models;

namespace UseCases.Appeals.Queries.GetAppealsByAdmin;

public class GetAppealsByAdminQuery : IRequest<IEnumerable<AppealResponse>>
{
    public int Since { get; set; }
    public int Count { get; set; }
}
