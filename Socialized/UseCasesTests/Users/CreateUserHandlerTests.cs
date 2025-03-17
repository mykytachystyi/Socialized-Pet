using Domain.Users;
using NSubstitute;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Emails;
using UseCases.Users.Commands.CreateUser;
using NSubstitute.ReturnsExtensions;
using Core.Providers.Hmac;
using Core.Providers.TextEncrypt;

namespace UseCasesTests.Users;

public class CreateUserHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IUserRepository userRepository = Substitute.For<IUserRepository>();
    private IEncryptionProvider encryptionProvider = Substitute.For<IEncryptionProvider>();
    private IEmailMessanger emailMessanger = Substitute.For<IEmailMessanger>();
    private TextEncryptionProvider profileCondition = Substitute.For<TextEncryptionProvider>();

    [Fact]
    public async Task Create_WhenUserIsAlreadyExistAndNotDeleted_ThrowNotFoundException()
    {
        var command = new CreateUserCommand
        {
            Email = "test@test.com",
            FirstName = "Rick",
            LastName = "Dolton",
            Password = "Pass1234!",
            CountryName = "USA",
            TimeZone = 6,
            Culture = "en_EN"
        };
        var user = new User { Email = command.Email, IsDeleted = false };
        userRepository.GetByEmail(command.Email).Returns(user);
        var handler = new CreateUserCommandHandler(userRepository, emailMessanger, encryptionProvider, profileCondition, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Create_WhenUserWasDeleted_RestoreUserAndReturn()
    {
        var command = new CreateUserCommand
        {
            Email = "test@test.com",
            FirstName = "Rick",
            LastName = "Dolton",
            Password = "Pass1234!",
            CountryName = "USA",
            TimeZone = 6,
            Culture = "en_EN"
        };
        var user = new User { Email = command.Email, IsDeleted = true };
        userRepository.GetByEmail(command.Email).Returns(user);
        var handler = new CreateUserCommandHandler(userRepository, emailMessanger, encryptionProvider, profileCondition, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
    [Fact]
    public async Task Create_WhenJustNewUser_CreateNewUserAndReturn()
    {
        var command = new CreateUserCommand
        {
            Email = "test@test.com",
            FirstName = "Rick",
            LastName = "Dolton",
            Password = "Pass1234!",
            CountryName = "USA",
            TimeZone = 6,
            Culture = "en_EN"
        };
        userRepository.GetByEmail(command.Email).ReturnsNull();
        var handler = new CreateUserCommandHandler(userRepository, emailMessanger, encryptionProvider, profileCondition, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
}
