using Core.Providers.Rand;
using Domain.Users;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Exceptions;
using UseCases.Users.Commands.LogOut;

namespace UseCasesTests.Users;

public class LogOutHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IRepository<User> userRepository = Substitute.For<IRepository<User>>();
    private readonly IRandomizer randomizer = Substitute.For<IRandomizer>();
    
    [Fact]
    public async Task Logout_WhenUserTokenIsFound_Return()
    {
        var command = new LogOutCommand { UserToken = "1234567890" };
        var user = new User { TokenForUse = command.UserToken };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
        var handler = new LogOutCommandHandler(userRepository, randomizer, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
    [Fact]
    public async Task Logout_WhenUserTokenIsNotFound_ThrowNotFoundException()
    {
        var command = new LogOutCommand { UserToken = "1234567890" };
        var user = new User { TokenForUse = command.UserToken };
        userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
        var handler = new LogOutCommandHandler(userRepository, randomizer, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}