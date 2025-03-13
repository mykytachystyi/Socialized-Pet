using AutoMapper;
using Domain.Appeals;
using Domain.Appeals.Repositories;
using Domain.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Appeals.Commands.CreateAppeal;
using UseCases.Appeals.Models;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals
{
    public class CreateAppealTests
    {
        private ILogger logger = Substitute.For<ILogger>();
        private IAppealRepository appealRepository = Substitute.For<IAppealRepository>();
        private IUserRepository userRepository = Substitute.For<IUserRepository>();
        private IMapper mapper = Substitute.For<IMapper>();

        [Fact]
        public async Task Create_WhenUserTokenAndIdIsValid_ReturnMessage()
        {
            var command = new CreateAppealCommand { Subject = "Test", UserToken = "1234567890" };
            var response = new AppealResponse { Subject = command.Subject, State = 1 };
            mapper.Map<AppealResponse>(null).ReturnsForAnyArgs(response);
            userRepository.GetByUserTokenNotDeleted(command.UserToken).Returns(new User { Id = 1 });
            var handler = new CreateAppealCommandHandler(userRepository, appealRepository, logger, mapper);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(result.Subject, command.Subject);
            Assert.Equal(1, result.State);
        }
        [Fact]
        public async Task Create_WhenUserTokenAndIdIsNotValid_ThrowNotFoundException()
        {
            var command = new CreateAppealCommand { Subject = "Test", UserToken = "1234567890" };
            userRepository.GetByUserTokenNotDeleted(command.UserToken).ReturnsNull();
            var handler = new CreateAppealCommandHandler(userRepository, appealRepository, logger, mapper);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}