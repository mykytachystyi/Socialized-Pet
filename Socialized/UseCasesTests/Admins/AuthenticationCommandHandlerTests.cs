using Core;
using Core.Providers.Hmac;
using Domain.Admins;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Admins.Commands.Authentication;
using UseCases.Exceptions;

namespace UseCasesTests.Admins;

public class AuthenticationCommandHandlerTests
{
    private readonly ILogger logger = Substitute.For<ILogger>();
    private readonly IRepository<Admin> repository = Substitute.For<IRepository<Admin>>();
    private readonly IEncryptionProvider encryptionProvider = Substitute.For<IEncryptionProvider>();

    [Fact]
    public async Task Authentication_WhenEmailIsFoundAndPasswordIsValid_ReturnAdmin()
    {
        var command = new AuthenticationCommand
        {
            Email = "test@test.com", Password = "password"
        };
        var adminHashed = new Admin 
        { 
            Id = 1, FirstName = "", 
            LastName = "", Role = "", 
            TokenForStart = "", Email = command.Email, 
            HashedPassword = new byte[1], HashedSalt = new byte[1]
        };
        encryptionProvider.VerifyPasswordHash(command.Password, Arg.Any<SaltAndHash>()).Returns(true);
        repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Admin, bool>>?>()).Returns(adminHashed);
        var handler = new AuthenticationCommandHandler(repository, logger, encryptionProvider);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(command.Email, result.Email);
    }
    [Fact]
    public async Task Authentication_WhenEmailIsNotFound_ThrowNotFoundException()
    {
        var command = new AuthenticationCommand
        {
            Email = "test@test.com",
            Password = "password"
        };
        repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Admin, bool>>?>()).ReturnsNull();
        var handler = new AuthenticationCommandHandler(repository, logger, encryptionProvider);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Authentication_WhenPasswordIsNotValid_ThrowValidationExceptionException()
    {
        var command = new AuthenticationCommand
        {
            Email = "test@test.com",
            Password = "password"
        };
        encryptionProvider.VerifyPasswordHash(command.Password, Arg.Any<SaltAndHash>()).Returns(false);
        var adminHashed = new Admin
        {
            Id = 1,
            FirstName = "",
            LastName = "",
            Role = "",
            TokenForStart = "",
            Email = command.Email,
            HashedPassword = new byte[1],
            HashedSalt = new byte[1]
        };
        repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Admin, bool>>?>()).Returns(adminHashed);
        var handler = new AuthenticationCommandHandler(repository, logger, encryptionProvider);

        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
    }

}
