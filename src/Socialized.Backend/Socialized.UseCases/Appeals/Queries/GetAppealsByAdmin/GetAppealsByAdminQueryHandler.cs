using Mapster;
using Domain.Appeals;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UseCases.Appeals.Models;

namespace UseCases.Appeals.Queries.GetAppealsByAdmin;

public class GetAppealsByAdminQueryHandler (
    IRepository<Appeal> appealRepository,
    ILogger logger) : IRequestHandler<GetAppealsByAdminQuery, IEnumerable<AppealResponse>>
{
    public async Task<IEnumerable<AppealResponse>> Handle(GetAppealsByAdminQuery request, 
        CancellationToken cancellationToken)
    {
        var appeals = appealRepository.AsNoTracking();

        var appealArray = await appeals.OrderBy(appeal => appeal.State)
            .ThenByDescending(appeal => appeal.CreatedAt)
            .Skip(request.Since * request.Count)
            .Take(request.Count)
            .ToArrayAsync();

        logger.Information($"Отримано список адміном, з={request.Since} по={request.Count}.");

        return appealArray.Adapt<List<AppealResponse>>();
    }
}
