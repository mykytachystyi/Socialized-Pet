using Domain.Users;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Exceptions;
using UseCases.Users.Commands.RegistrationEmail;
using UseCases.Users.Emails;

namespace UseCasesTests.Users;

public class RegistrationEmailCommandHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<User> userRepository = Substitute.For<IRepository<User>>();
    private IEmailMessanger emailMessanger = Substitute.For<IEmailMessanger>();

    [Fact]
    public async Task RegistrationEmail_WhenUserIsNotFoundByEmail_ThrowNotFoundException()
    {
        // Arrange
        var command = new RegistrationEmailCommand { UserEmail = "test@test.com", Culture = "en_EN" };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
        var handler = new RegistrationEmailCommandHandler(userRepository, emailMessanger, logger);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task RegistrationEmail_WhenUserIsFoundByEmailValid_SendEmailAndReturn()
    {
        // Arrange
        var command = new RegistrationEmailCommand { UserEmail = "test@test.com", Culture = "en_EN" };
        var user = new User { Email = command.UserEmail, IsDeleted = true };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        var handler = new RegistrationEmailCommandHandler(userRepository, emailMessanger, logger);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }
}