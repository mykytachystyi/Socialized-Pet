using Domain.Appeals;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Appeals.Replies.Commands.UpdateAppealMessageReply;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals;

public class UpdateReplyHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<AppealMessageReply> replyRepository = Substitute.For<IRepository<AppealMessageReply>>();
    
    [Fact]
    public async Task Update_WhenReplyIsNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new UpdateAppealMessageReplyCommand { ReplyId = 1, Reply = "Updated Reply" };
        replyRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessageReply, bool>>?>()).ReturnsNull();

        var handler = new UpdateAppealMessageReplyCommandHandler(replyRepository, logger);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Update_WhenReplyIsFound_UpdatesReply()
    {
        // Arrange
        var command = new UpdateAppealMessageReplyCommand { ReplyId = 1, Reply = "Updated Reply" };
        var reply = new AppealMessageReply { Id = 1, Reply = "Old Reply", Message = new AppealMessage() };
        replyRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessageReply, bool>>?>()).Returns(reply);

        var handler = new UpdateAppealMessageReplyCommandHandler(replyRepository, logger);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }
}