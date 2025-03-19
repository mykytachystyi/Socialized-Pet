using Serilog;
using NSubstitute;
using UseCases.Exceptions;
using Domain.Appeals;
using NSubstitute.ReturnsExtensions;
using UseCases.Appeals.Replies.Commands.DeleteAppealMessageReply;
using Infrastructure.Repositories;
using System.Linq.Expressions;

namespace UseCasesTests.Appeals.Replies;

public class DeleteReplyHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<AppealMessageReply> replyRepository = Substitute.For<IRepository<AppealMessageReply>>();
    
    [Fact]
    public async Task Delete_WhenReplyIsNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new DeleteAppealMessageReplyCommand { ReplyId = 1 };
        replyRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessageReply, bool>>?>()).ReturnsNull();
        
        var handler = new DeleteAppealMessageReplyCommandHandler(replyRepository, logger);


        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Delete_WhenReplyIsFound_DeletesReply()
    {
        // Arrange
        var command = new DeleteAppealMessageReplyCommand { ReplyId = 1 };
        var reply = new AppealMessageReply { Id = 1, Reply = "", IsDeleted = false, Message = new AppealMessage() };
        replyRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessageReply, bool>>?>()).Returns(reply);

        var handler = new DeleteAppealMessageReplyCommandHandler(replyRepository, logger);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }
}