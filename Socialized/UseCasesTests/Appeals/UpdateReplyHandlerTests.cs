using Domain.Appeals;
using Domain.Appeals.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Appeals.Replies.Commands.UpdateAppealMessageReply;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals;

public class UpdateReplyHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IAppealMessageReplyRepository replyRepository = Substitute.For<IAppealMessageReplyRepository>();
    
    [Fact]
    public async Task Update_WhenReplyIsNotFound_ThrowsNotFoundException()
    {
        var command = new UpdateAppealMessageReplyCommand { ReplyId = 1, Reply = "Updated Reply" };
        replyRepository.Get(command.ReplyId).ReturnsNull();

        var handler = new UpdateAppealMessageReplyCommandHandler(replyRepository, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public void Update_WhenReplyIsFound_UpdatesReply()
    {
        var command = new UpdateAppealMessageReplyCommand { ReplyId = 1, Reply = "Updated Reply" };
        var reply = new AppealMessageReply { Id = 1, Reply = "Old Reply", Message = new AppealMessage() };
        replyRepository.Get(command.ReplyId).Returns(reply);

        var handler = new UpdateAppealMessageReplyCommandHandler(replyRepository, logger);

        Assert.Equal(command.Reply, reply.Reply);
    }
}