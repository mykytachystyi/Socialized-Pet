using AutoMapper;
using Domain.Appeals;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Appeals.Replies.Commands.CreateAppealMessageReply;
using UseCases.Appeals.Replies.Models;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals;

public class CreateReplyHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<Appeal> appealRepository = Substitute.For<IRepository<Appeal>>();
    private IRepository<AppealMessage> messageRepository = Substitute.For<IRepository<AppealMessage>>();
    private IRepository<AppealMessageReply> replyRepository = Substitute.For<IRepository<AppealMessageReply>>();
    private IMapper mapper = Substitute.For<IMapper>();

    [Fact]
    public async Task Create_WhenMessageIsNotFound_ThrowsNotFoundException()
    {
        var command = new CreateAppealMessageReplyCommand { AppealMessageId = 1, Reply = "Test Reply" };
        messageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>()).ReturnsNull();
        
        var handler = new CreateAppealMessageReplyCommandHandler(messageRepository, appealRepository, 
            replyRepository, logger, mapper);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Create_WhenMessageIsFound_CreatesReply()
    {
        var command = new CreateAppealMessageReplyCommand { AppealMessageId = 1, Reply = "Test Reply" };
        var appeal = new Appeal { Id = 1 };
        var message = new AppealMessage { Id = 1, AppealId = appeal.Id };
        var response = new AppealReplyResponse { Reply = command.Reply };
        mapper.Map<AppealReplyResponse>(null).ReturnsForAnyArgs(response);
        messageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>()).Returns(message);
        appealRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Appeal, bool>>?>()).Returns(appeal);
        var handler = new CreateAppealMessageReplyCommandHandler(messageRepository, appealRepository, replyRepository, logger, mapper);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(command.Reply, result.Reply);
    }
}
