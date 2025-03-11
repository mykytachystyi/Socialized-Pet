using AutoMapper;
using Domain.Appeals;
using Domain.Appeals.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Appeals.Replies.Commands.CreateAppealMessageReply;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals;

public class CreateReplyHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IAppealRepository appealRepository = Substitute.For<IAppealRepository>();
    private IAppealMessageRepository messageRepository = Substitute.For<IAppealMessageRepository>();
    private IAppealMessageReplyRepository replyRepository = Substitute.For<IAppealMessageReplyRepository>();
    private IMapper mapper = Substitute.For<IMapper>();

    [Fact]
    public async Task Create_WhenMessageIsNotFound_ThrowsNotFoundException()
    {
        var command = new CreateAppealMessageReplyCommand { AppealMessageId = 1, Reply = "Test Reply" };
        messageRepository.GetBy(command.AppealMessageId).ReturnsNull();

        var handler = new CreateAppealMessageReplyCommandHandler(messageRepository, appealRepository, replyRepository, logger, mapper);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Create_WhenMessageIsFound_CreatesReply()
    {
        var command = new CreateAppealMessageReplyCommand { AppealMessageId = 1, Reply = "Test Reply" };
        var message = new AppealMessage { Id = 1, AppealId = 1 };
        messageRepository.GetBy(command.AppealMessageId).Returns(message);

        var handler = new CreateAppealMessageReplyCommandHandler(messageRepository, appealRepository, replyRepository, logger, mapper);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(command.Reply, result.Reply);
    }
}
