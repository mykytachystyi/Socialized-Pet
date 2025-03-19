using AutoMapper;
using Domain.Admins;
using Domain.Appeals;
using MediatR;
using Serilog;
using UseCases.Exceptions;
using UseCases.Appeals.Replies.Models;
using Infrastructure.Repositories;

namespace UseCases.Appeals.Replies.Commands.CreateAppealMessageReply;

public class CreateAppealMessageReplyCommandHandler (
    IRepository<AppealMessage> messageRepository,
    IRepository<Appeal> appealRepository,
    IRepository<AppealMessageReply> replyRepository,
    ILogger logger,
    IMapper mapper
    ) : IRequestHandler<CreateAppealMessageReplyCommand, AppealReplyResponse>
{
    public async Task<AppealReplyResponse> Handle(CreateAppealMessageReplyCommand request, 
        CancellationToken cancellationToken)
    {
        logger.Information("Початок створення відповіді на повідомлення.");
        var message = await messageRepository.FirstOrDefaultAsync(m => m.Id == request.AppealMessageId);
        if (message == null)
        {
            throw new NotFoundException("Повідомлення не було визначенно сервером по id.");
        }

        var appeal = await appealRepository.FirstOrDefaultAsync(a => a.Id == message.AppealId);
        appeal.State = (int) AppealState.Answered;    
        logger.Information("Звернення було оновленно, статус завернення - опрацьовано.");

        var reply = new AppealMessageReply
        {
            AppealMessageId = message.Id,
            Reply = request.Reply,
            Message = message,
            CreatedAt = DateTime.UtcNow,
        };
        await replyRepository.AddAsync(reply);
        logger.Information($"Було створенно відповідь на повідомлення, id={reply.Id}.");
        return mapper.Map<AppealReplyResponse>(reply);
    }
}
