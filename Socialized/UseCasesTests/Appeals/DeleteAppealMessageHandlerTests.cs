using Domain.Appeals;
using Domain.Appeals.Repositories;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Appeals.Messages.DeleteAppealMessage;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals;

public class DeleteAppealMessageHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<AppealMessage> appealMessageRepository = Substitute.For<IRepository<AppealMessage>>();
    
    [Fact]
    public async Task Delete_WhenIdIsNotFound_ThrowNotFoundException()
    {
        var command = new DeleteAppealMessageCommand
        {
            MessageId = 1
        };
        appealMessageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>()).ReturnsNull();
        var handler = new DeleteAppealMessageCommandHandler(appealMessageRepository, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Delete_WhenIdIsFound_Return()
    {
        var command = new DeleteAppealMessageCommand
        {
            MessageId = 1
        };
        appealMessageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>())
            .Returns(new AppealMessage { Id = command.MessageId, Message = "text" });
        var handler = new DeleteAppealMessageCommandHandler(appealMessageRepository, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
}