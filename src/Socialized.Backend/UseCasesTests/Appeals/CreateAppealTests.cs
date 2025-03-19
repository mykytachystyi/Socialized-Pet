using AutoMapper;
using Domain.Appeals;
using Domain.Users;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
using UseCases.Appeals.Commands.CreateAppeal;
using UseCases.Appeals.Models;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals
{
    public class CreateAppealTests
    {
        private ILogger logger = Substitute.For<ILogger>();
        private IRepository<Appeal> appealRepository = Substitute.For<IRepository<Appeal>>();
        private IRepository<User> userRepository = Substitute.For<IRepository<User>>();
        private IMapper mapper = Substitute.For<IMapper>();

        [Fact]
        public async Task Create_WhenUserTokenAndIdIsValid_ReturnMessage()
        {
            // Arrange
            var command = new CreateAppealCommand { Subject = "Test", UserToken = "1234567890" };
            var response = new AppealResponse { Subject = command.Subject, State = 1 };
            mapper.Map<AppealResponse>(null).ReturnsForAnyArgs(response);
            userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(new User { Id = 1 });
            var handler = new CreateAppealCommandHandler(appealRepository, userRepository, logger, mapper);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(result.Subject, command.Subject);
            Assert.Equal(1, result.State);
        }
        [Fact]
        public async Task Create_WhenUserTokenAndIdIsNotValid_ThrowNotFoundException()
        {
            // Arrange
            var command = new CreateAppealCommand { Subject = "Test", UserToken = "1234567890" };
            userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).ReturnsNull();
            var handler = new CreateAppealCommandHandler(appealRepository, userRepository, logger, mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}