using AutoMapper;
using Core.Providers.Hmac;
using Core.Providers.Rand;
using Core.Providers.TextEncrypt;
using Domain.Admins;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Admins.Commands.CreateAdmin;
using UseCases.Admins.Emails;
using UseCases.Admins.Models;
using UseCases.Exceptions;

namespace UseCasesTests.Admins;

public class CreateAdminTests
{
    private readonly ILogger logger = Substitute.For<ILogger>();
    private readonly IAdminRepository repository = Substitute.For<IAdminRepository>();
    private readonly IAdminEmailManager emailManager = Substitute.For<IAdminEmailManager>();
    private readonly IMapper mapper = Substitute.For<IMapper>();
    private readonly IEncryptionProvider encryptionProvider = Substitute.For<IEncryptionProvider>();
    private readonly IRandomizer randomizer = Substitute.For<IRandomizer>();
    private readonly Admin admin = new Admin
    { 
        Email = "", FirstName = "", 
        LastName = "", HashedPassword = new byte[0], 
        HashedSalt = new byte[0], 
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
        var admin = new AdminResponse { Email = command.Email, FirstName = command.FirstName, LastName = command.LastName, Role = "default" };
        mapper.Map<AdminResponse>(null).ReturnsForAnyArgs(admin);
        var handler = new CreateAdminCommandHandler(repository, 
            encryptionProvider, emailManager, logger, randomizer, mapper);

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
        var handler = new CreateAdminCommandHandler(repository, encryptionProvider,
            emailManager, logger, randomizer, mapper);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}