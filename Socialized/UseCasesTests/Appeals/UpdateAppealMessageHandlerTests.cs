using Domain.Appeals;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Appeals.Messages.UpdateAppealMessage;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals;

public class UpdateAppealMessageHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<AppealMessage> appealMessageRepository = Substitute.For<IRepository<AppealMessage>>();
    
    [Fact]
    public async Task Update_WhenAppealIdIsNotFound_ThrowNotFoundException()
    {
        var command = new UpdateAppealMessageCommand
        {
            MessageId = 1,
            Message = "update test",
        };
        appealMessageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>()).ReturnsNull();
        var handler = new UpdateAppealMessageCommandHandler(appealMessageRepository, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Update_WhenAppealIdIsFound_Return()
    {
        var command = new UpdateAppealMessageCommand
        {
            MessageId = 1,
            Message = "update test",
        };
        appealMessageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>())
            .Returns(new AppealMessage { Id = command.MessageId, Message = "text" });
        var handler = new UpdateAppealMessageCommandHandler(appealMessageRepository, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
}