using AutoMapper;
using Domain.Appeals.Repositories;
using MediatR;
using Serilog;
using UseCases.Appeals.Models;

namespace UseCases.Appeals.Queries.GetAppealsByAdmin;

public class GetAppealsByAdminQueryHandler (
    IAppealRepository appealRepository,
    ILogger logger,
    IMapper mapper) : IRequestHandler<GetAppealsByAdminQuery, IEnumerable<AppealResponse>>
{
    public async Task<IEnumerable<AppealResponse>> Handle(GetAppealsByAdminQuery request, 
        CancellationToken cancellationToken)
    {
        var appeals = appealRepository.GetAppealsBy(request.Since, request.Count);

        logger.Information($"Отримано список адміном, з={request.Since} по={request.Count}.");

        return mapper.Map<List<AppealResponse>>(appeals);
    }
}
