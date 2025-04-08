using MediatR;
using Serilog;
using Mapster;
using UseCases.Appeals.Models;
using Domain.Appeals.Repositories;

namespace UseCases.Appeals.Queries.GetAppealsByUser;

public class GetAppealsByUserQueryHandler(
    ILogger logger,
    IAppealQueryRepository appealRepository) : IRequestHandler<GetAppealsByUserQuery, IEnumerable<AppealResponse>>
{
    public async Task<IEnumerable<AppealResponse>> Handle(GetAppealsByUserQuery request, 
        CancellationToken cancellationToken)
    {
        logger.Information($"Отримано список користувачем, з={request.Since} по={request.Count}.");

        var appeals = await appealRepository.GetAppealsByAsync(request.UserId, request.Since, request.Count);

        return appeals.Adapt<List<AppealResponse>>();
    }
}