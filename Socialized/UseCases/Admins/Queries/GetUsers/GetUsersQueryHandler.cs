using AutoMapper;
using Domain.Admins;
using MediatR;
using Serilog;
using UseCases.Response;

namespace UseCases.Admins.Queries.GetUsers;

public class GetUsersQueryHandler(
    ILogger logger,
    IAdminRepository adminRepository,
    IMapper mapper
    ) : IRequestHandler<GetUsersQuery, IEnumerable<UserResponse>>
{
    public async Task<IEnumerable<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        logger.Information($"Отримано список користувачів, з={request.Since} по={request.Count}.");

        var users = adminRepository.GetUsers(request.Since, request.Count);

        return mapper.Map<List<UserResponse>>(users);
    }
}