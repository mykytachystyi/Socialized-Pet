using Core.Providers.Rand;
using Domain.Users;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Exceptions;
using UseCases.Users.Commands.CheckRecoveryCode;

namespace UseCasesTests.Users;

public class CheckRecoveryCodeCommandHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<User> userRepository = Substitute.For<IRepository<User>>();
    private readonly IRandomizer randomizer = Substitute.For<IRandomizer>();

    [Fact]
    public async Task CheckRecoveryCode_WhenCodeIsFoundAndEmailIsFound_Return()
    {
        // Arrange
        var command = new CheckRecoveryCodeCommand { UserEmail = "test@test.com", RecoveryCode = 1111 };
        var user = new User
        {
            Email = command.UserEmail,
            IsDeleted = false,
            RecoveryCode = command.RecoveryCode
        };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        randomizer.CreateHash(40).Returns("1234567890123456789012345678901234567890");
        var handler = new CheckRecoveryCodeCommandHandler(logger, userRepository, randomizer);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.RecoveryToken);
    }
    [Fact]
    public async Task CheckRecoveryCode_WhenEmailIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        var command = new CheckRecoveryCodeCommand { UserEmail = "test@test.com", RecoveryCode = 1111 };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
        var handler = new CheckRecoveryCodeCommandHandler(logger, userRepository, randomizer);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task CheckRecoveryCode_WhenCodeIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        var command = new CheckRecoveryCodeCommand { UserEmail = "test@test.com", RecoveryCode = 1111 };
        var user = new User
        {
            Email = command.UserEmail,
            IsDeleted = false,
            RecoveryCode = 2222
        };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        var handler = new CheckRecoveryCodeCommandHandler(logger, userRepository, randomizer);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
    }
}