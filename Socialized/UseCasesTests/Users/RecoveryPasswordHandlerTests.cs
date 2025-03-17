using Core.Providers.Rand;
using Domain.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Commands.RecoveryPassword;
using UseCases.Users.Emails;

namespace UseCasesTests.Users;

public class RecoveryPasswordHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IUserRepository userRepository = Substitute.For<IUserRepository>();
    private IEmailMessanger emailMessanger = Substitute.For<IEmailMessanger>();
    private readonly IRandomizer randomizer = Substitute.For<IRandomizer>();

    [Fact]
    public async Task RecoveryPassword_WhenEmailIsFound_Return()
    {
        var command = new RecoveryPasswordCommand { UserEmail = "test@test.com", Culture = "en_EN" };
        var user = new User { Email = command.UserEmail, IsDeleted = false };
        userRepository.GetByEmail(command.UserEmail, false, true).Returns(user);
        var handler = new RecoveryPasswordCommandHandler(userRepository, randomizer, emailMessanger, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
    [Fact]
    public async Task RecoveryPassword_WhenEmailIsNotFound_ThrowNotFoundException()
    {
        var command = new RecoveryPasswordCommand { UserEmail = "test@test.com", Culture = "en_EN" };
        userRepository.GetByEmail(command.UserEmail, false, true).ReturnsNull();
        var handler = new RecoveryPasswordCommandHandler(userRepository, randomizer, emailMessanger, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}