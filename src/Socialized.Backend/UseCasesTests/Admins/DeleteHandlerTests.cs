using Domain.Admins;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Admins.Commands.Delete;
using UseCases.Exceptions;

namespace UseCasesTests.Admins;

public class DeleteHandlerTests
{
    private readonly ILogger logger = Substitute.For<ILogger>();
    private readonly IRepository<Admin> repository = Substitute.For<IRepository<Admin>>();
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
    public async Task Delete_WhenIdIsFound_Return()
    {
        // Arrange
        var command = new DeleteAdminCommand { AdminId = 1 };
        repository.GetByIdAsync(Arg.Any<long>()).Returns(admin);
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
        repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Admin, bool>>?>()).ReturnsNull();
        var handler = new DeleteAdminCommandHandler(repository, logger);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}