using MediatR;
using UseCases.Admins.Models;

namespace UseCases.Admins.Queries.GetAdmins;

public class GetAdminsQuery : IRequest<IEnumerable<AdminResponse>>
{
    public long AdminId { get; set; }
    public int Since { get; set; }
    public int Count { get; set; }
}