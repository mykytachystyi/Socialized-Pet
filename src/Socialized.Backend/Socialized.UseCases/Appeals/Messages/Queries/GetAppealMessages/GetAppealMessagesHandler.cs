﻿using Mapster;
using Domain.Appeals;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UseCases.Appeals.Messages.Models;

namespace UseCases.Appeals.Messages.Queries.GetAppealMessages;

public class GetAppealMessagesHandler (
    ILogger logger,
    IRepository<AppealMessage> appealMessageRepository
    )
    : IRequestHandler<GetAppealMessagesCommand, IEnumerable<AppealMessageResponse>>
{
    public Task<IEnumerable<AppealMessageResponse>> Handle(GetAppealMessagesCommand request, CancellationToken cancellationToken)
    {
        logger.Information($"Starting to get appeal messages for appeal {request.AppealId}.");

        var messages = appealMessageRepository.AsNoTracking()
            .Include(message => message.Files)
            .Where(message => message.AppealId == request.AppealId
                && !message.IsDeleted)
            .OrderByDescending(message => message.UpdatedAt)
            .Skip(request.Since * request.Count)
            .Take(request.Count)
            .ToArray();

        logger.Information($"Getting appeal messages for appeal {request.AppealId}.");

        return Task.FromResult<IEnumerable<AppealMessageResponse>>(messages.Adapt<List<AppealMessageResponse>>());
    }
}