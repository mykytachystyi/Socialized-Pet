using Domain.Appeals;
using Domain.Appeals.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Appeals.Messages.UpdateAppealMessage;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals;

public class UpdateAppealMessageHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IAppealMessageRepository appealMessageRepository = Substitute.For<IAppealMessageRepository>();
    
    [Fact]
    public async Task Update_WhenAppealIdIsNotFound_ThrowNotFoundException()
    {
        var command = new UpdateAppealMessageCommand
        {
            MessageId = 1,
            Message = "update test",
        };
        appealMessageRepository.GetBy(command.MessageId).ReturnsNull();
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
        appealMessageRepository.GetBy(command.MessageId)
            .Returns(new AppealMessage { Id = command.MessageId, Message = "text" });
        var handler = new UpdateAppealMessageCommandHandler(appealMessageRepository, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
}