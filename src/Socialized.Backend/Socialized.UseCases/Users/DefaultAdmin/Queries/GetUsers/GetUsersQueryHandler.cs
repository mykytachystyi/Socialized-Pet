using Mapster;
using Domain.Users;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UseCases.Users.DefaultAdmin.Models;

namespace UseCases.Users.DefaultAdmin.Queries.GetUsers;

public class GetUsersQueryHandler(
    ILogger logger,
    IRepository<User> userRepository
    ) : IRequestHandler<GetUsersQuery, IEnumerable<UserResponse>>
{
    public async Task<IEnumerable<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        logger.Information($"Отримано список користувачів, з={request.Since} по={request.Count}.");

        var users = userRepository.AsNoTracking();

        var usersArray = await users.Where(u => !u.IsDeleted && u.Id != request.AdminId && u.Role == request.Role).OrderByDescending(u => u.Id)
                .Skip(request.Since * request.Count)
                .Take(request.Count)
                .ToArrayAsync();

        return usersArray.Adapt<List<UserResponse>>();
    }
}