using Domain.Admins;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Admins.Commands.Delete;
using UseCases.Exceptions;

namespace UseCasesTests.Admins;

public class DeleteHandlerTests
{
    private readonly ILogger logger = Substitute.For<ILogger>();
    private readonly IAdminRepository repository = Substitute.For<IAdminRepository>();
    private readonly Admin admin = new Admin
    {
        Email = "",
        FirstName = "",
        LastName = "",
        Password = "",
        Role = "",
        TokenForStart = ""
    };
    [Fact]
    public async Task Delete_WhenIdIsFound_Return()
    {
        var command = new DeleteAdminCommand { AdminId = 1 };
        repository.GetByAdminId(command.AdminId, false).Returns(admin);
        var handler = new DeleteAdminCommandHandler(repository, logger);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
    }
    [Fact]
    public async Task Delete_WhenIdIsNotFound_ThrowNotFoundException()
    {
        var command = new DeleteAdminCommand { AdminId = 1 };
        repository.GetByAdminId(command.AdminId, false).ReturnsNull();
        var handler = new DeleteAdminCommandHandler(repository, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}