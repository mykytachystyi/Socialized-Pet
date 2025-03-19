using Core;
using Core.Providers.Hmac;
using Domain.Admins;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Admins.Commands.SetupPassword;
using UseCases.Exceptions;

namespace UseCasesTests.Admins
{
    public class SetupPasswordTests
    {
        private readonly ILogger logger = Substitute.For<ILogger>();
        private readonly IRepository<Admin> repository = Substitute.For<IRepository<Admin>>();
        private readonly IEncryptionProvider encryptionProvider = Substitute.For<IEncryptionProvider>();
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
        public async Task SetupPassword_WhenAdminTokenIsFound_Return()
        {
            // Arrange
            var command = new SetupPasswordCommand { Token = "1234567890", Password = "password" };
            repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Admin, bool>>?>()).Returns(admin);
            var handler = new SetupPasswordCommandHandler(logger, repository, encryptionProvider);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
        }
        [Fact]
        public async Task SetupPassword_WhenTokenIsNotFound_ThrowNotFoundException()
        {
            // Arrange
            var command = new SetupPasswordCommand { Token = "1234567890", Password = "password" };
            repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Admin, bool>>?>()).ReturnsNull();
            var handler = new SetupPasswordCommandHandler(logger, repository, encryptionProvider);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}