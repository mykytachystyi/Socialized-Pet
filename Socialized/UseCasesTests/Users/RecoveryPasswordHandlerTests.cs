using Core.Providers.Rand;
using Domain.Users;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Exceptions;
using UseCases.Users.Commands.RecoveryPassword;
using UseCases.Users.Emails;

namespace UseCasesTests.Users;

public class RecoveryPasswordHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<User> userRepository = Substitute.For<IRepository<User>>();
    private IEmailMessanger emailMessanger = Substitute.For<IEmailMessanger>();
    private readonly IRandomizer randomizer = Substitute.For<IRandomizer>();

    [Fact]
    public async Task RecoveryPassword_WhenEmailIsFound_Return()
    {
        // Arrange
        var command = new RecoveryPasswordCommand { UserEmail = "test@test.com", Culture = "en_EN" };
        var user = new User { Email = command.UserEmail, IsDeleted = false };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        var handler = new RecoveryPasswordCommandHandler(userRepository, randomizer, emailMessanger, logger);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }
    [Fact]
    public async Task RecoveryPassword_WhenEmailIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        var command = new RecoveryPasswordCommand { UserEmail = "test@test.com", Culture = "en_EN" };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
        var handler = new RecoveryPasswordCommandHandler(userRepository, randomizer, emailMessanger, logger);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}