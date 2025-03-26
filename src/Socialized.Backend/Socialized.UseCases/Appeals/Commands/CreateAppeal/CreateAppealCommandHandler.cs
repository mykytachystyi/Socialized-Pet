using Serilog;
using Domain.Appeals;
using Domain.Users;
using MediatR;
using System.Web;
using UseCases.Exceptions;
using AutoMapper;
using UseCases.Appeals.Models;
using Infrastructure.Repositories;

namespace UseCases.Appeals.Commands.CreateAppeal;

public class CreateAppealCommandHandler(
    IRepository<Appeal> appealRepository,
    IRepository<User> userRepository,
    ILogger logger,
    IMapper mapper
    ) : IRequestHandler<CreateAppealWithUserCommand, AppealResponse>
{
    public async Task<AppealResponse> Handle(CreateAppealWithUserCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Початок створення нової заяви.");

        var user = await userRepository.FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted);
        if (user == null)
        {
            throw new NotFoundException("Користувач не був визначений по id.");
        }
        var appeal = new Appeal
        {
            UserId = user.Id,
            Subject = HttpUtility.UrlDecode(request.Subject),
            State = 1,
            CreatedAt = DateTime.UtcNow,
            LastActivity = DateTime.UtcNow
        };
        await appealRepository.AddAsync(appeal);

        logger.Information($"Було створенно нова заява, id={appeal.Id}.");
        return mapper.Map<AppealResponse>(appeal);
    }
}