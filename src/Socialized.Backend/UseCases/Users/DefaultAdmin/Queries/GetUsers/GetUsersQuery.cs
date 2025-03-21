using MediatR;
using UseCases.Users.DefaultAdmin.Models;

namespace UseCases.Users.DefaultAdmin.Queries.GetUsers;

public class GetUsersQuery : IRequest<IEnumerable<UserResponse>>
{
    public int Role { get; set; }
    public long AdminId { get; set; }
    public int Since { get; set; }
    public int Count { get; set; }
}