using AutoMapper;
using Domain.Appeals;
using Domain.Appeals.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Appeals.Replies.Commands.CreateAppealMessageReply;
using UseCases.Appeals.Replies.Models;
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
        var appeal = new Appeal { Id = 1 };
        var message = new AppealMessage { Id = 1, AppealId = appeal.Id };
        var response = new AppealReplyResponse { Reply = command.Reply };
        mapper.Map<AppealReplyResponse>(null).ReturnsForAnyArgs(response);
        messageRepository.GetBy(command.AppealMessageId).Returns(message);
        appealRepository.GetBy(appeal.Id).Returns(appeal);

        var handler = new CreateAppealMessageReplyCommandHandler(messageRepository, appealRepository, replyRepository, logger, mapper);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(command.Reply, result.Reply);
    }
}
