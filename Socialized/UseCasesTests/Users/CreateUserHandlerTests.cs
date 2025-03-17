using Domain.Users;
using NSubstitute;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Emails;
using UseCases.Users.Commands.CreateUser;
using NSubstitute.ReturnsExtensions;
using Core.Providers.Hmac;
using Core.Providers.Rand;
using Infrastructure.Repositories;
using System.Linq.Expressions;

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
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        var handler = new CreateUserCommandHandler(userRepository, emailMessanger, encryptionProvider, randomizer, logger);

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
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        var handler = new CreateUserCommandHandler(userRepository, emailMessanger, encryptionProvider, randomizer, logger);

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
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
        var handler = new CreateUserCommandHandler(userRepository, emailMessanger, encryptionProvider, randomizer, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
}
