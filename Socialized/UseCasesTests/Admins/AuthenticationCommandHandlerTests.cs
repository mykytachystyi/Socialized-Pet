using AutoMapper;
using Core;
using Domain.Admins;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Admins.Commands.Authentication;
using UseCases.Admins.Emails;
using UseCases.Admins.Models;
using UseCases.Exceptions;

namespace UseCasesTests.Admins;

public class AuthenticationCommandHandlerTests
{
    private readonly ILogger logger = Substitute.For<ILogger>();
    private readonly IAdminRepository repository = Substitute.For<IAdminRepository>();
    private readonly IAdminEmailManager emailManager = Substitute.For<IAdminEmailManager>();
    private readonly IMapper mapper = Substitute.For<IMapper>();
    private readonly ProfileCondition profileCondition = new ProfileCondition();

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
            Password = profileCondition.HashPassword(command.Password)
        };
        repository.GetByEmail(command.Email).Returns(adminHashed);
        mapper.Map<AdminResponse>(adminHashed).Returns(
            new AdminResponse 
            { 
                Email = adminHashed.Email,
                FirstName = adminHashed.FirstName,
                LastName = adminHashed.LastName,
                Role = adminHashed.Role
            });
        var handler = new AuthenticationCommandHandler(repository, logger, profileCondition, mapper);

        var result = handler.Handle(command, CancellationToken.None);

        Assert.Equal(command.Email, result.Result.Email);
    }
    [Fact]
    public async Task Authentication_WhenEmailIsNotFound_ThrowNotFoundException()
    {
        var command = new AuthenticationCommand
        {
            Email = "test@test.com",
            Password = "password"
        };
        repository.GetByEmail(command.Email).ReturnsNull();
        var handler = new AuthenticationCommandHandler(repository, logger, profileCondition, mapper);

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
        var hashedPassword = profileCondition.HashPassword("different_password");
        var admin = new Admin { Id = 1, FirstName = "", LastName = "", Role = "", TokenForStart = "", Email = command.Email, Password = hashedPassword };
        repository.GetByEmail(command.Email).Returns(admin);
        var handler = new AuthenticationCommandHandler(repository, logger, profileCondition, mapper);

        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
    }

}
