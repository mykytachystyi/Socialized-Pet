using AutoMapper;
using Domain.Appeals;
using Domain.Users;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using System.Web;
using UseCases.Appeals.Files.CreateAppealMessageFile;
using UseCases.Appeals.Messages.Models;
using UseCases.Exceptions;

namespace UseCases.Appeals.Messages.CreateAppealMessage
{
    public class CreateAppealMessageCommandHandler(
        IRepository<User> userRepository,
        IRepository<Appeal> appealRepository,
        IRepository<AppealMessage> appealMessageRepository,
        ILogger logger,
        IMapper mapper,
        ICreateAppealFilesAdditionalToMessage filesAdditionalToMessage
        ) : IRequestHandler<CreateAppealMessageWithUserCommand, AppealMessageResponse>
    {
        public async Task<AppealMessageResponse> Handle(CreateAppealMessageWithUserCommand request, 
            CancellationToken cancellationToken)
        {
            var user = await userRepository.FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted);

            if (user == null)
            {
                throw new NotFoundException("Користувач не був визначений по токену.");
            }
            var appeal = await appealRepository.FirstOrDefaultAsync(a => a.Id == request.AppealId && a.UserId == user.Id);
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
            await appealMessageRepository.AddAsync(message);
            logger.Information($"Створено було повідомлення в зверненні, id={message.Id}.");

            if (request.Files != null)
            {
                message.Files = filesAdditionalToMessage.Create(request.Files, message);
            }
            return mapper.Map<AppealMessageResponse>(message);
        }
    }
}