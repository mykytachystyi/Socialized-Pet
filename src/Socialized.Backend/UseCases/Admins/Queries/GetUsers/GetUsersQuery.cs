using MediatR;
using UseCases.Users.Models;

namespace UseCases.Admins.Queries.GetUsers;

public class GetUsersQuery : IRequest<IEnumerable<UserResponse>>
{
    public int Since { get; set; }
    public int Count { get; set; }
}