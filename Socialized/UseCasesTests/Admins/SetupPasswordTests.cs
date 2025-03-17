using Core;
using Core.Providers.Hmac;
using Domain.Admins;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Admins.Commands.SetupPassword;
using UseCases.Exceptions;

namespace UseCasesTests.Admins
{
    public class SetupPasswordTests
    {
        private readonly ILogger logger = Substitute.For<ILogger>();
        private readonly IAdminRepository repository = Substitute.For<IAdminRepository>();
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
            var command = new SetupPasswordCommand { Token = "1234567890", Password = "password" };
            repository.GetByPasswordToken(command.Token, false).Returns(admin);
            var handler = new SetupPasswordCommandHandler(logger, repository, encryptionProvider);

            await handler.Handle(command, CancellationToken.None);
        }
        [Fact]
        public async Task SetupPassword_WhenTokenIsNotFound_ThrowNotFoundException()
        {
            var command = new SetupPasswordCommand { Token = "1234567890", Password = "password" };
            repository.GetByPasswordToken(command.Token, false).ReturnsNull();
            var handler = new SetupPasswordCommandHandler(logger, repository, encryptionProvider);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}