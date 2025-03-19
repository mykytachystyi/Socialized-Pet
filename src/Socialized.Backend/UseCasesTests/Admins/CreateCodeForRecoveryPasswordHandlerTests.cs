using Core.Providers.Rand;
using Domain.Admins;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Admins.Commands.CreateCodeForRecoveryPassword;
using UseCases.Admins.Emails;
using UseCases.Exceptions;

namespace UseCasesTests.Admins;

public class CreateCodeForRecoveryPasswordHandlerTests
{
    private readonly ILogger logger = Substitute.For<ILogger>();
    private readonly IRepository<Admin> repository = Substitute.For<IRepository<Admin>>();
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
        // Arrange
        var command = new CreateCodeForRecoveryPasswordCommand
        {
            AdminEmail = "test@test.com"
        };
        admin.Email = command.AdminEmail;
        repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Admin, bool>>?>()).Returns(admin);

        var handler = new CreateCodeForRecoveryPasswordHandler(repository, emailManager, logger, randomizer);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }
    [Fact]
    public async Task CreateCodeForRecoveryPassword_WhenEmailIsNotFound_Return()
    {
        // Arrange
        var command = new CreateCodeForRecoveryPasswordCommand
        {
            AdminEmail = "test@test.com"
        };
        repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Admin, bool>>?>()).ReturnsNull();

        var handler = new CreateCodeForRecoveryPasswordHandler(repository, emailManager, logger, randomizer);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}