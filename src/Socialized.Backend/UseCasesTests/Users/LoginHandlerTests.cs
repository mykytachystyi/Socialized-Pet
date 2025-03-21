using Serilog;
using NSubstitute;
using Domain.Users;
using UseCases.Exceptions;
using NSubstitute.ReturnsExtensions;
using Core.Providers.Hmac;
using Infrastructure.Repositories;
using System.Linq.Expressions;
using UseCases.Users.DefaultUser.Commands.LoginUser;

namespace UseCasesTests.Users;

public class LoginHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<User> userRepository = Substitute.For<IRepository<User>>();
    private IEncryptionProvider encryptionProvider = Substitute.For<IEncryptionProvider>();

    [Fact]
    public async Task Login_WhenUserPasswordValidAndEmailIsFound_ReturnUser()
    {
        // Arrange
        var command = new LoginUserWithRoleCommand
        {
            Email = "test@test.com",
            Password = "password",
            Role = 1
        };
        var hashedPassword = new SaltAndHash { Hash = new byte[1], Salt = new byte[1] };
        var user = new User 
        { 
            Email = command.Email, 
            HashedPassword = hashedPassword.Hash, 
            HashedSalt = hashedPassword.Salt,
            Role = command.Role
        };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        encryptionProvider.VerifyPasswordHash(command.Password, Arg.Any<SaltAndHash>()).Returns(true);
        var handler = new LoginUserCommandHandler(userRepository, encryptionProvider, logger);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(command.Email, result.Email);
    }
    [Fact]
    public async Task Login_WhenUserTryToLoginWithDifferentRole_ThrowValidationException()
    {
        // Arrange
        var command = new LoginUserWithRoleCommand
        {
            Email = "test@test.com",
            Password = "password",
            Role = 2
        };
        var hashedPassword = new SaltAndHash { Hash = new byte[1], Salt = new byte[1] };
        var user = new User
        {
            Email = command.Email,
            HashedPassword = hashedPassword.Hash,
            HashedSalt = hashedPassword.Salt,
            Role = 1
        };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        encryptionProvider.VerifyPasswordHash(command.Password, Arg.Any<SaltAndHash>()).Returns(true);
        var handler = new LoginUserCommandHandler(userRepository, encryptionProvider, logger);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

    }
    [Fact]
    public async Task Login_WhenUserPasswordIsNotValid_ThrowValidationException()
    {
        // Arrange
        var command = new LoginUserWithRoleCommand
        {
            Email = "test@test.com",
            Password = "password",
            Role = 1
        };
        var hashedPassword = new SaltAndHash { Hash = new byte[1], Salt = new byte[1] };
        encryptionProvider.VerifyPasswordHash(command.Password, Arg.Any<SaltAndHash>()).Returns(false);
        var user = new User
        {
            Email = command.Email,
            HashedPassword = hashedPassword.Hash,
            HashedSalt = hashedPassword.Salt,
            Role = 1
        };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        var handler = new LoginUserCommandHandler(userRepository, encryptionProvider, logger);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Login_WhenUserEmailIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        var command = new LoginUserWithRoleCommand
        {
            Email = "test@test.com",
            Password = "password",
            Role = 1
        };
        var hashedPassword = new SaltAndHash { Hash = new byte[1], Salt = new byte[1] };
        var user = new User
        {
            Email = command.Email,
            HashedPassword = hashedPassword.Hash,
            HashedSalt = hashedPassword.Salt,
            Role = 1
        };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
        var handler = new LoginUserCommandHandler(userRepository, encryptionProvider, logger);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}