using AutoMapper;
using Domain.Appeals;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using UseCases.Appeals.Models;

namespace UseCases.Appeals.Queries.GetAppealsByAdmin;

public class GetAppealsByAdminQueryHandler (
    IRepository<Appeal> appealRepository,
    ILogger logger,
    IMapper mapper) : IRequestHandler<GetAppealsByAdminQuery, IEnumerable<AppealResponse>>
{
    public async Task<IEnumerable<AppealResponse>> Handle(GetAppealsByAdminQuery request, 
        CancellationToken cancellationToken)
    {
        var appeals = appealRepository.AsNoTracking();

        var appealArray = appeals.OrderBy(appeal => appeal.State)
            .ThenByDescending(appeal => appeal.CreatedAt)
            .Skip(request.Since * request.Count)
            .Take(request.Count)
            .ToArray();

        logger.Information($"Отримано список адміном, з={request.Since} по={request.Count}.");

        return mapper.Map<List<AppealResponse>>(appealArray);
    }
}
