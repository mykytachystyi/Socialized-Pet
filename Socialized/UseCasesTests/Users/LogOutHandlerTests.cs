using Core;
using Domain.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Commands.LogOut;

namespace UseCasesTests.Users;

public class LogOutHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IUserRepository userRepository = Substitute.For<IUserRepository>();
    private ProfileCondition profileCondition = new ProfileCondition();

    [Fact]
    public async Task Logout_WhenUserTokenIsFound_Return()
    {
        var command = new LogOutCommand { UserToken = "1234567890" };
        var user = new User { TokenForUse = command.UserToken };
        userRepository.GetByUserTokenNotDeleted(command.UserToken).Returns(user);
        var handler = new LogOutCommandHandler(userRepository, profileCondition, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
    [Fact]
    public async Task Logout_WhenUserTokenIsNotFound_ThrowNotFoundException()
    {
        var command = new LogOutCommand { UserToken = "1234567890" };
        var user = new User { TokenForUse = command.UserToken };
        userRepository.GetByUserTokenNotDeleted(command.UserToken).ReturnsNull();
        var handler = new LogOutCommandHandler(userRepository, profileCondition, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}