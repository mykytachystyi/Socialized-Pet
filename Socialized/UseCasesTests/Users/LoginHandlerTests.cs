using Core;
using Serilog;
using AutoMapper;
using NSubstitute;
using Domain.Users;
using UseCases.Exceptions;
using NSubstitute.ReturnsExtensions;
using UseCases.Users.Models;
using UseCases.Users.Commands.LoginUser;

namespace UseCasesTests.Users
{
    public class LoginHandlerTests
    {
        private ILogger logger = Substitute.For<ILogger>();
        private IUserRepository userRepository = Substitute.For<IUserRepository>();
        private IMapper mapper = Substitute.For<IMapper>();
        private ProfileCondition profileCondition = new ProfileCondition();

        [Fact]
        public async Task Login_WhenUserPasswordValidAndEmailIsFound_ReturnUser()
        {
            var command = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var hashedPassword = profileCondition.HashPassword(command.Password);
            var user = new User { Email = command.Email, Password = hashedPassword };
            userRepository.GetByEmail(command.Email).Returns(user);
            var handler = new LoginUserCommandHandler(userRepository, profileCondition, mapper, logger);
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
            var hashedPassword = profileCondition.HashPassword("different_password");
            var user = new User { Email = command.Email, Password = hashedPassword };
            userRepository.GetByEmail(command.Email).Returns(user);
            var handler = new LoginUserCommandHandler(userRepository, profileCondition, mapper, logger);

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
            var hashedPassword = profileCondition.HashPassword("different_password");
            var user = new User { Email = command.Email, Password = hashedPassword };
            userRepository.GetByEmail(command.Email).ReturnsNull();
            var handler = new LoginUserCommandHandler(userRepository, profileCondition, mapper, logger);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}