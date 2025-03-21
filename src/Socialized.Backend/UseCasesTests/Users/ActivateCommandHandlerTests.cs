using Serilog;
using Domain.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using UseCases.Exceptions;
using Infrastructure.Repositories;
using System.Linq.Expressions;
using UseCases.Users.DefaultUser.Commands.Activate;

namespace UseCasesTests.Users;

public class ActivateCommandHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<User> userRepository = Substitute.For<IRepository<User>>();
    [Fact]
    public async Task Activate_WhenUserIsNotFoundByToken_ThrowNotFoundException()
    {
        // Arrange
        var command = new ActivateCommand { Hash = "1234567890" };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
        var handler = new ActivateCommandHandler(userRepository, logger);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Activate_WhenUserIsFoundByToken_ActivateAndReturn()
    {
        // Arrange
        var command = new ActivateCommand { Hash = "1234567890" };
        var user = new User { Email = "test@test.com", IsDeleted = true };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        var handler = new ActivateCommandHandler(userRepository, logger);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }
}