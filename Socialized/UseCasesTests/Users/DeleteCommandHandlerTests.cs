using Domain.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Commands.Delete;

namespace UseCasesTests.Users;

public class DeleteCommandHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IUserRepository userRepository = Substitute.For<IUserRepository>();

    [Fact]
    public async Task Delete_WhenUserIsNotFoundByToken_ThrowNotFoundException()
    {
        var command = new DeleteCommand { UserToken = "1234567890" };
        userRepository.GetByUserTokenNotDeleted(command.UserToken).ReturnsNull();
        var handler = new DeleteCommandHandler(logger, userRepository);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Delete_WhenUserIsFoundByToken_DeleteAndReturn()
    {
        var command = new DeleteCommand { UserToken = "1234567890" };
        var user = new User { Email = "test@test.com", IsDeleted = false };
        userRepository.GetByUserTokenNotDeleted(command.UserToken).Returns(user);
        var handler = new DeleteCommandHandler(logger, userRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
}