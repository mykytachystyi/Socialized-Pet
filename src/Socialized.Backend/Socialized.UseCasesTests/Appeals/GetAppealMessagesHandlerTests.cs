using Domain.Appeals;
using FluentAssertions;
using Infrastructure.Repositories;
using Serilog;
using NSubstitute;
using UseCases.Appeals.Messages.Queries.GetAppealMessages;

namespace UseCasesTests.Appeals;

public class GetAppealMessagesHandlerTests
{
    [Fact]
    public async Task Handle_GivenValidRequest_ShouldReturnAppealMessages()
    {
        // Arrange
        var logger = Substitute.For<ILogger>();
        var repository = Substitute.For<IRepository<AppealMessage>>();
        var handler = new GetAppealMessagesHandler(logger, repository);
        var request = new GetAppealMessagesCommand
        {
            AppealId = 1,
            Since = 0,
            Count = 10
        };

        var messages = new List<AppealMessage>
        {
            new AppealMessage { Id = 1, AppealId = 1, Message = "Message 1" },
            new AppealMessage { Id = 2, AppealId = 1, Message = "Message 2" }
        }.AsQueryable();
        repository.AsNoTracking().Returns(messages);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainSingle(m => m.Id == 1 && m.Message == "Message 1");
        result.Should().ContainSingle(m => m.Id == 2 && m.Message == "Message 2");
    }
}
