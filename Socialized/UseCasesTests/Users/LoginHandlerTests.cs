using Core;
using Serilog;
using AutoMapper;
using NSubstitute;
using Domain.Users;
using UseCases.Exceptions;
using NSubstitute.ReturnsExtensions;
using UseCases.Users.Models;
using UseCases.Users.Commands.LoginUser;
using Core.Providers.Hmac;
using Infrastructure.Repositories;
using System.Linq.Expressions;

namespace UseCasesTests.Users
{
    public class LoginHandlerTests
    {
        private ILogger logger = Substitute.For<ILogger>();
        private IRepository<User> userRepository = Substitute.For<IRepository<User>>();
        private IMapper mapper = Substitute.For<IMapper>();
        private IEncryptionProvider encryptionProvider = Substitute.For<IEncryptionProvider>();

        [Fact]
        public async Task Login_WhenUserPasswordValidAndEmailIsFound_ReturnUser()
        {
            var command = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var hashedPassword = new SaltAndHash { Hash = new byte[1], Salt = new byte[1] };
            var user = new User 
            { 
                Email = command.Email, 
                HashedPassword = hashedPassword.Hash, 
                HashedSalt = hashedPassword.Salt 
            };
            userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
            encryptionProvider.VerifyPasswordHash(command.Password, Arg.Any<SaltAndHash>()).Returns(true);
            var handler = new LoginUserCommandHandler(userRepository, encryptionProvider, mapper, logger);
            mapper.Map<UserResponse>(user).Returns(new UserResponse { Email = command.Email, FirstName = "", LastName = "", TokenForUse = "" });

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(command.Email, result.Email);
        }
        [Fact]
        public async Task Login_WhenUserPasswordIsNotValid_ThrowValidationException()
        {
            var command = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var hashedPassword = new SaltAndHash { Hash = new byte[1], Salt = new byte[1] };
            encryptionProvider.VerifyPasswordHash(command.Password, Arg.Any<SaltAndHash>()).Returns(false);
            var user = new User
            {
                Email = command.Email,
                HashedPassword = hashedPassword.Hash,
                HashedSalt = hashedPassword.Salt
            };
            userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(user);
            var handler = new LoginUserCommandHandler(userRepository, encryptionProvider, mapper, logger);

            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }
        [Fact]
        public async Task Login_WhenUserEmailIsNotFound_ThrowNotFoundException()
        {
            var command = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var hashedPassword = new SaltAndHash { Hash = new byte[1], Salt = new byte[1] };
            var user = new User
            {
                Email = command.Email,
                HashedPassword = hashedPassword.Hash,
                HashedSalt = hashedPassword.Salt
            };
            userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
            var handler = new LoginUserCommandHandler(userRepository, encryptionProvider, mapper, logger);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}