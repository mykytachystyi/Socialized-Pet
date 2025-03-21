using Domain.Enums;
using Domain.Users;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Exceptions;
using UseCases.Users.DefaultAdmin.Commands.DeleteAdmin;

namespace UseCasesTests.Admins;

public class DeleteHandlerTests
{
    private readonly ILogger logger = Substitute.For<ILogger>();
    private readonly IRepository<User> repository = Substitute.For<IRepository<User>>();
    private readonly User admin = new User
    {
        Email = "",
        FirstName = "",
        LastName = "",
        HashedPassword = new byte[0],
        HashedSalt = new byte[0],
        Role = (int) IdentityRole.DefaultAdmin,
    };
    [Fact]
    public async Task Delete_WhenIdIsFound_Return()
    {
        // Arrange
        var command = new DeleteAdminCommand { AdminId = 1 };
        repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(admin);
        var handler = new DeleteAdminCommandHandler(repository, logger);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }
    [Fact]
    public async Task Delete_WhenIdIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        var command = new DeleteAdminCommand { AdminId = 1 };
        repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
        var handler = new DeleteAdminCommandHandler(repository, logger);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}