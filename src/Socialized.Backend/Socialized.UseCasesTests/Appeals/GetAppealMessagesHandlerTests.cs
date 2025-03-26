using AutoMapper;
using Domain.Appeals;
using FluentAssertions;
using Infrastructure.Repositories;
using Serilog;
using NSubstitute;
using UseCases.Appeals.Messages.Models;
using UseCases.Appeals.Messages.Queries.GetAppealMessages;
using Microsoft.EntityFrameworkCore;

namespace UseCasesTests.Appeals;

public class GetAppealMessagesHandlerTests
{
    [Fact]
    public async Task Handle_GivenValidRequest_ShouldReturnAppealMessages()
    {
        // Arrange
        var logger = Substitute.For<ILogger>();
        var repository = Substitute.For<IRepository<AppealMessage>>();
        var mapper = Substitute.For<IMapper>();
        var handler = new GetAppealMessagesHandler(logger, repository, mapper);
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

        mapper.Map<List<AppealMessageResponse>>(Arg.Any<Array>())
            .Returns(new List<AppealMessageResponse>
            {
                new AppealMessageResponse { Id = 1, Message = "Message 1" },
                new AppealMessageResponse { Id = 2, Message = "Message 2" }
            });
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainSingle(m => m.Id == 1 && m.Message == "Message 1");
        result.Should().ContainSingle(m => m.Id == 2 && m.Message == "Message 2");
    }
}
