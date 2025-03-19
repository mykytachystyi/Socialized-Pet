using Domain.Users;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Exceptions;
using UseCases.Users.Commands.Delete;

namespace UseCasesTests.Users;

public class DeleteCommandHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<User> userRepository = Substitute.For<IRepository<User>>();

    [Fact]
    public async Task Delete_WhenUserIsNotFoundByToken_ThrowNotFoundException()
    {
        // Arrange
        var command = new DeleteCommand { UserToken = "1234567890" };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
        var handler = new DeleteCommandHandler(logger, userRepository);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Delete_WhenUserIsFoundByToken_DeleteAndReturn()
    {
        // Arrange
        var command = new DeleteCommand { UserToken = "1234567890" };
        var user = new User { Email = "test@test.com", IsDeleted = false };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        var handler = new DeleteCommandHandler(logger, userRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }
}