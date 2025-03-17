using Core.Providers.Rand;
using Core.Providers.TextEncrypt;
using Domain.Admins;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Admins.Commands.CreateCodeForRecoveryPassword;
using UseCases.Admins.Emails;
using UseCases.Exceptions;

namespace UseCasesTests.Admins;

public class CreateCodeForRecoveryPasswordHandlerTests
{
    private readonly ILogger logger = Substitute.For<ILogger>();
    private readonly IAdminRepository repository = Substitute.For<IAdminRepository>();
    private readonly IAdminEmailManager emailManager = Substitute.For<IAdminEmailManager>();
    private readonly IRandomizer randomizer = Substitute.For<IRandomizer>();
    private readonly Admin admin = new Admin
    {
        Email = "",
        FirstName = "",
        LastName = "",
        HashedPassword = new byte[0],
        HashedSalt = new byte[0],
        Role = "",
        TokenForStart = ""
    };

    [Fact]
    public async Task CreateCodeForRecoveryPassword_WhenEmailIsFound_Return()
    {
        var command = new CreateCodeForRecoveryPasswordCommand
        {
            AdminEmail = "test@test.com"
        };
        admin.Email = command.AdminEmail;
        repository.GetByEmail(command.AdminEmail, false).Returns(admin);
        var handler = new CreateCodeForRecoveryPasswordHandler(repository, emailManager, logger, randomizer);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
    [Fact]
    public async Task CreateCodeForRecoveryPassword_WhenEmailIsNotFound_Return()
    {
        var command = new CreateCodeForRecoveryPasswordCommand
        {
            AdminEmail = "test@test.com"
        };
        repository.GetByEmail(command.AdminEmail, false).ReturnsNull();
        var handler = new CreateCodeForRecoveryPasswordHandler(repository, emailManager, logger, randomizer);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}