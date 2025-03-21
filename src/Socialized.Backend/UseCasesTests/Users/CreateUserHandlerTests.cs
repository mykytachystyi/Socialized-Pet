using Domain.Users;
using NSubstitute;
using Serilog;
using UseCases.Exceptions;
using NSubstitute.ReturnsExtensions;
using Core.Providers.Hmac;
using Core.Providers.Rand;
using Infrastructure.Repositories;
using System.Linq.Expressions;
using UseCases.Users.DefaultUser.Emails;
using UseCases.Users.DefaultUser.Commands.CreateUser;
using Domain.Enums;

namespace UseCasesTests.Users;

public class CreateUserHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<User> userRepository = Substitute.For<IRepository<User>>();
    private IEncryptionProvider encryptionProvider = Substitute.For<IEncryptionProvider>();
    private IEmailMessanger emailMessanger = Substitute.For<IEmailMessanger>();
    private readonly IRandomizer randomizer = Substitute.For<IRandomizer>();

    [Fact]
    public async Task Create_WhenUserIsAlreadyExistAndNotDeleted_ThrowNotFoundException()
    {
        // Arrange
        var command = new CreateUserWithRoleCommand((int)IdentityRole.DefaultUser, "en_EN")
        {
            Email = "test@test.com",
            FirstName = "Rick",
            LastName = "Dolton",
            Password = "Pass1234!"
        };
        var user = new User { Email = command.Email, IsDeleted = false };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        var handler = new CreateUserCommandHandler(userRepository, emailMessanger, encryptionProvider, randomizer, logger);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Create_WhenUserWasDeleted_RestoreUserAndReturn()
    {
        // Arrange
        var command = new CreateUserWithRoleCommand((int)IdentityRole.DefaultUser, "en_EN")
        {
            Email = "test@test.com",
            FirstName = "Rick",
            LastName = "Dolton",
            Password = "Pass1234!"
        };
        var user = new User { Email = command.Email, IsDeleted = true };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        var handler = new CreateUserCommandHandler(userRepository, emailMessanger, encryptionProvider, randomizer, logger);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }
    [Fact]
    public async Task Create_WhenJustNewUser_CreateNewUserAndReturn()
    {
        // Arrange
        var command = new CreateUserWithRoleCommand((int)IdentityRole.DefaultUser, "en_EN")
        {
            Email = "test@test.com",
            FirstName = "Rick",
            LastName = "Dolton",
            Password = "Pass1234!"
        };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
        var handler = new CreateUserCommandHandler(userRepository, emailMessanger, encryptionProvider, randomizer, logger);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }
}
