﻿using Core.Providers.Hmac;
using Domain.Users;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Exceptions;
using UseCases.Users.DefaultUser.Commands.ChangePassword;

namespace UseCasesTests.Users
{
    public class ChangeUserPasswordHandlerTests
    {
        private ILogger logger = Substitute.For<ILogger>();
        private IRepository<User> userRepository = Substitute.For<IRepository<User>>();
        private IEncryptionProvider encryptionProvider = Substitute.For<IEncryptionProvider>();

        [Fact]
        public async Task ChangePassword_WhenTokenIsFoundAndPasswordIsValid_Return()
        {
            // Arrange
            var command = new ChangeUserPasswordCommand
            {
                RecoveryToken = "1234567890",
                UserPassword = "Pass1234!",
                UserConfirmPassword = "Pass1234!"
            };
            var user = new User { IsDeleted = false, RecoveryToken = command.RecoveryToken };
            userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
            var handler = new ChangeUserPasswordCommandHandler(userRepository, encryptionProvider, logger);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
        }
        [Fact]
        public async Task ChangePassword_WhenTokenIsNotFound_ThrowNotFoundException()
        {
            // Arrange
            var command = new ChangeUserPasswordCommand
            {
                RecoveryToken = "1234567890",
                UserPassword = "Pass1234!",
                UserConfirmPassword = "Pass1234!"
            };
            userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
            var handler = new ChangeUserPasswordCommandHandler(userRepository, encryptionProvider, logger);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
        [Fact]
        public async Task ChangePassword_WhenPasswordIsNotValid_ThrowValidationException()
        {
            // Arrange
            var command = new ChangeUserPasswordCommand
            {
                RecoveryToken = "1234567890",
                UserPassword = "Pass1234!",
                UserConfirmPassword = ""
            };
            var user = new User { IsDeleted = false, RecoveryToken = command.RecoveryToken };
            userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
            var handler = new ChangeUserPasswordCommandHandler(userRepository, encryptionProvider, logger);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
