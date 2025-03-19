using MediatR;
using Serilog;
using AutoMapper;
using Domain.Admins;
using UseCases.Admins.Models;
using Infrastructure.Repositories;

namespace UseCases.Admins.Queries.GetAdmins;

public class GetAdminsQueryHandler(
    ILogger logger, 
    IRepository<Admin> adminRepository,
    IMapper mapper) : IRequestHandler<GetAdminsQuery, IEnumerable<AdminResponse>>
{
    public async Task<IEnumerable<AdminResponse>> Handle(GetAdminsQuery request, CancellationToken cancellationToken)
    {
        logger.Information($"Отримано список адмінів, з={request.Since} по={request.Count} адміном id={request.AdminId}.");

        var admins = adminRepository.AsNoTracking();

        var adminsArray = admins.Where(a => a.Id != request.AdminId && !a.IsDeleted)
                .Skip(request.Since * request.Count)
                .Take(request.Count)
                .ToArray();

        return mapper.Map<List<AdminResponse>>(adminsArray);
    }
}