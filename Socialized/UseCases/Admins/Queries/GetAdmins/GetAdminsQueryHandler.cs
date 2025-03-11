using MediatR;
using Serilog;
using AutoMapper;
using Domain.Admins;
using UseCases.Admins.Models;

namespace UseCases.Admins.Queries.GetAdmins;

public class GetAdminsQueryHandler(
    ILogger logger, 
    IAdminRepository adminRepository,
    IMapper mapper) : IRequestHandler<GetAdminsQuery, IEnumerable<AdminResponse>>
{
    public async Task<IEnumerable<AdminResponse>> Handle(GetAdminsQuery request, CancellationToken cancellationToken)
    {
        logger.Information($"Отримано список адмінів, з={request.Since} по={request.Count} адміном id={request.AdminId}.");
        
        var admins = adminRepository.GetActiveAdmins(request.AdminId, request.Since, request.Count);

        return mapper.Map<List<AdminResponse>>(admins);
    }
}