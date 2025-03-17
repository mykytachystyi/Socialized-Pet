using MediatR;
using Serilog;
using AutoMapper;
using UseCases.Appeals.Models;
using Domain.Appeals.Repositories;

namespace UseCases.Appeals.Queries.GetAppealsByUser;

public class GetAppealsByUserQueryHandler(
    ILogger logger,
    IAppealQueryRepository appealRepository,
    IMapper mapper) : IRequestHandler<GetAppealsByUserQuery, IEnumerable<AppealResponse>>
{
    public async Task<IEnumerable<AppealResponse>> Handle(GetAppealsByUserQuery request, 
        CancellationToken cancellationToken)
    {
        logger.Information($"Отримано список користувачем, з={request.Since} по={request.Count}.");

        var appeals = appealRepository.GetAppealsBy(request.UserToken, request.Since, request.Count);

        return mapper.Map<List<AppealResponse>>(appeals);
    }
}