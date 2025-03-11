using AutoMapper;
using Domain.Appeals;
using Domain.Appeals.Repositories;
using MediatR;
using Serilog;
using System.Web;
using UseCases.Appeals.Messages.Models;
using UseCases.Exceptions;

namespace UseCases.Appeals.Messages.CreateAppealMessage
{
    public class CreateAppealMessageCommandHandler(
        IAppealRepository appealRepository,
        IAppealMessageRepository appealMessageRepository,
        ILogger logger,
        IMapper mapper) : IRequestHandler<CreateAppealMessageCommand, AppealMessageResponse>
    {
        public async Task<AppealMessageResponse> Handle(CreateAppealMessageCommand request, 
            CancellationToken cancellationToken)
        {
            var appeal = appealRepository.GetBy(request.AppealId, request.UserToken);
            if (appeal == null)
            {
                throw new NotFoundException("Звернення не було визначенно сервером по id.");
            }
            var message = new AppealMessage()
            {
                AppealId = appeal.Id,
                Message = string.IsNullOrEmpty(request.Message) ? "" : HttpUtility.UrlDecode(request.Message),
                CreatedAt = DateTime.UtcNow,
            };
            appealMessageRepository.Create(message);
            logger.Information($"Створено було повідомлення в зверненні, id={message.Id}.");

            if (request.Files != null)
            {
                message.Files = appealFileManager.Create(command.Files, message.Id);
            }
            return mapper.Map<AppealMessageResponse>(message);
        }
    }
}
