using Serilog;
using Domain.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using UseCases.Exceptions;
using UseCases.Users.Commands.Activate;

namespace UseCasesTests.Users;

public class ActivateCommandHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IUserRepository userRepository = Substitute.For<IUserRepository>();
    [Fact]
    public async Task Activate_WhenUserIsNotFoundByToken_ThrowNotFoundException()
    {
        var command = new ActivateCommand { Hash = "1234567890" };
        userRepository.GetByHash(command.Hash, false, false).ReturnsNull();
        var handler = new ActivateCommandHandler(userRepository, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Activate_WhenUserIsFoundByToken_ActivateAndReturn()
    {
        var command = new ActivateCommand { Hash = "1234567890" };
        var user = new User { Email = "test@test.com", IsDeleted = true };
        userRepository.GetByHash(command.Hash, false, false).Returns(user);
        var handler = new ActivateCommandHandler(userRepository, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
}