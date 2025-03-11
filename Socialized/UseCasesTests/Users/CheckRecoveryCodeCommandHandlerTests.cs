using Core;
using Domain.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Commands.CheckRecoveryCode;

namespace UseCasesTests.Users;

public class CheckRecoveryCodeCommandHandlerTests
{
    private ILogger logger = Substitute.For<ILogger>();
    private IUserRepository userRepository = Substitute.For<IUserRepository>();
    private ProfileCondition profileCondition = new ProfileCondition();

    [Fact]
    public async Task CheckRecoveryCode_WhenCodeIsFoundAndEmailIsFound_Return()
    {
        var command = new CheckRecoveryCodeCommand { UserEmail = "test@test.com", RecoveryCode = 1111 };
        var user = new User
        {
            Email = command.UserEmail,
            IsDeleted = false,
            RecoveryCode = command.RecoveryCode
        };
        userRepository.GetByEmail(command.UserEmail).Returns(user);
        var handler = new CheckRecoveryCodeCommandHandler(logger, userRepository, profileCondition);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotEmpty(result.RecoveryToken);
    }
    [Fact]
    public async Task CheckRecoveryCode_WhenEmailIsNotFound_ThrowNotFoundException()
    {
        var command = new CheckRecoveryCodeCommand { UserEmail = "test@test.com", RecoveryCode = 1111 };
        userRepository.GetByEmail(command.UserEmail).ReturnsNull();
        var handler = new CheckRecoveryCodeCommandHandler(logger, userRepository, profileCondition);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task CheckRecoveryCode_WhenCodeIsNotFound_ThrowNotFoundException()
    {
        var command = new CheckRecoveryCodeCommand { UserEmail = "test@test.com", RecoveryCode = 1111 };
        var user = new User
        {
            Email = command.UserEmail,
            IsDeleted = false,
            RecoveryCode = 2222
        };
        userRepository.GetByEmail(command.UserEmail).Returns(user);
        var handler = new CheckRecoveryCodeCommandHandler(logger, userRepository, profileCondition);

        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
    }
}