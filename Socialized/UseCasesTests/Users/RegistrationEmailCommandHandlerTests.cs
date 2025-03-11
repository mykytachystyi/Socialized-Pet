using Domain.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Commands.RegistrationEmail;
using UseCases.Users.Emails;

namespace UseCasesTests.Users;

public class RegistrationEmailCommandHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IUserRepository userRepository = Substitute.For<IUserRepository>();
    private IEmailMessanger emailMessanger = Substitute.For<IEmailMessanger>();

    [Fact]
    public async Task RegistrationEmail_WhenUserIsNotFoundByEmail_ThrowNotFoundException()
    {
        var command = new RegistrationEmailCommand { UserEmail = "test@test.com", Culture = "en_EN" };
        userRepository.GetByEmail(command.UserEmail).ReturnsNull();
        var handler = new RegistrationEmailCommandHandler(userRepository, emailMessanger, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task RegistrationEmail_WhenUserIsFoundByEmailValid_SendEmailAndReturn()
    {
        var command = new RegistrationEmailCommand { UserEmail = "test@test.com", Culture = "en_EN" };
        var user = new User { Email = command.UserEmail, IsDeleted = true };
        userRepository.GetByEmail(command.UserEmail).Returns(user);
        var handler = new RegistrationEmailCommandHandler(userRepository, emailMessanger, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
}