using AutoMapper;
using Domain.Users;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using UseCases.Users.Models;

namespace UseCases.Admins.Queries.GetUsers;

public class GetUsersQueryHandler(
    ILogger logger,
    IRepository<User> userRepository,
    IMapper mapper
    ) : IRequestHandler<GetUsersQuery, IEnumerable<UserResponse>>
{
    public async Task<IEnumerable<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        logger.Information($"Отримано список користувачів, з={request.Since} по={request.Count}.");

        var users = userRepository.AsNoTracking();

        var usersArray = users.Where(u => !u.IsDeleted && u.Activate).OrderByDescending(u => u.Id)
                .Skip(request.Since * request.Count)
                .Take(request.Count)
                .ToArray();

        return mapper.Map<List<UserResponse>>(users);
    }
}