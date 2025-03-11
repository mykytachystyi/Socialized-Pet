using Serilog;
using NSubstitute;
using UseCases.Exceptions;
using Domain.Appeals;
using NSubstitute.ReturnsExtensions;
using Domain.Appeals.Repositories;
using UseCases.Appeals.Replies.Commands.DeleteAppealMessageReply;

namespace UseCasesTests.Appeals.Replies;

public class DeleteReplyHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IAppealMessageReplyRepository replyRepository = Substitute.For<IAppealMessageReplyRepository>();
    
    [Fact]
    public async Task Delete_WhenReplyIsNotFound_ThrowsNotFoundException()
    {
        var command = new DeleteAppealMessageReplyCommand { ReplyId = 1 };
        replyRepository.Get(command.ReplyId).ReturnsNull();

        var handler = new DeleteAppealMessageReplyCommandHandler(replyRepository, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Delete_WhenReplyIsFound_DeletesReply()
    {
        var command = new DeleteAppealMessageReplyCommand { ReplyId = 1 };
        var reply = new AppealMessageReply { Id = 1, Reply = "", IsDeleted = false, Message = new AppealMessage() };
        replyRepository.Get(command.ReplyId).Returns(reply);

        var handler = new DeleteAppealMessageReplyCommandHandler(replyRepository, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
}