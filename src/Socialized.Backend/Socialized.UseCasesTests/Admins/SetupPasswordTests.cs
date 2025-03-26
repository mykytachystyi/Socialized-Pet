using Core.Providers.Hmac;
using Domain.Enums;
using Domain.Users;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Exceptions;
using UseCases.Users.DefaultAdmin.Commands.SetupPassword;

namespace UseCasesTests.Admins
{
    public class SetupPasswordTests
    {
        private readonly ILogger logger = Substitute.For<ILogger>();
        private readonly IRepository<User> repository = Substitute.For<IRepository<User>>();
        private readonly IEncryptionProvider encryptionProvider = Substitute.For<IEncryptionProvider>();
        private readonly User admin = new User
        {
            Email = "",
            FirstName = "",
            LastName = "",
            HashedPassword = new byte[0],
            HashedSalt = new byte[0],
            Role = (int) IdentityRole.DefaultAdmin
        };
        [Fact]
        public async Task SetupPassword_WhenAdminTokenIsFound_Return()
        {
            // Arrange
            var command = new SetupPasswordWithAdminCommand { AdminId = 1, Password = "password" };
            repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(admin);
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
            var command = new SetupPasswordWithAdminCommand { AdminId = 1, Password = "password" };
            repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
            var handler = new SetupPasswordCommandHandler(logger, repository, encryptionProvider);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}