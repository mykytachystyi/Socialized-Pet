using AutoMapper;
using Core;
using Domain.Admins;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Admins.Commands.CreateAdmin;
using UseCases.Admins.Emails;
using UseCases.Exceptions;

namespace UseCasesTests.Admins;

public class CreateAdminTests
{
    private readonly ILogger logger = Substitute.For<ILogger>();
    private readonly IAdminRepository repository = Substitute.For<IAdminRepository>();
    private readonly IAdminEmailManager emailManager = Substitute.For<IAdminEmailManager>();
    private readonly IMapper mapper = Substitute.For<IMapper>();
    private readonly ProfileCondition profileCondition = new ProfileCondition();
    private readonly Admin admin = new Admin
    { 
        Email = "", FirstName = "", 
        LastName = "", Password = "", 
        Role = "", TokenForStart = "" 
    };

    [Fact]
    public async Task Create_WhenSameEmailIsNotFound_ReturnNewAdmin()
    {
        var command = new CreateAdminCommand
        {
            Email = "test@test.com",
            FirstName = "Rick",
            LastName = "Dolton",
            Password = "password"
        };
        repository.GetByEmail(command.Email, false).ReturnsNull();

        var handler = new CreateAdminCommandHandler(repository, profileCondition, emailManager, logger, mapper);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(result.Email, command.Email);
        Assert.Equal(result.FirstName, command.FirstName);
        Assert.Equal(result.LastName, command.LastName);
    }
    [Fact]
    public async Task Create_WhenSameEmailIsFound_ThrowNotFoundException()
    {
        var command = new CreateAdminCommand
        {
            Email = "test@test.com",
            FirstName = "Rick",
            LastName = "Dolton",
            Password = "password"
        };
        repository.GetByEmail(command.Email, false).Returns(admin);
        var handler = new CreateAdminCommandHandler(repository, profileCondition, emailManager, logger, mapper);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}