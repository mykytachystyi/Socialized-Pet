using AutoMapper;
using Domain.Appeals;
using Domain.Users;
using Infrastructure.Repositories;
using NSubstitute;
using Serilog;
using System.Linq.Expressions;
using UseCases.Appeals.Files.CreateAppealMessageFile;
using UseCases.Appeals.Files.Models;
using UseCases.Appeals.Messages.CreateAppealMessage;
using UseCases.Appeals.Messages.Models;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals
{
    public class CreateAppealMessageHandlerTests
    {
        private FileDto File = new FileDto
        {
            FileName = "test",
            Name = "test",
            ContentType = "image",
            ContentDisposition = "form-data",
            Length = 3,
            Headers = new Dictionary<string, string> { { "test", "test" } },
            Stream = new MemoryStream()
        };
        private ILogger logger = Substitute.For<ILogger>();
        private IRepository<Appeal> appealRepository = Substitute.For<IRepository<Appeal>>();
        private IRepository<AppealMessage> appealMessageRepository = Substitute.For<IRepository<AppealMessage>>();
        private IRepository<User> userRepository = Substitute.For<IRepository<User>>();
        private IMapper mapper = Substitute.For<IMapper>();
        private ICreateAppealFilesAdditionalToMessage filesAdditionalToMessage = Substitute.For<ICreateAppealFilesAdditionalToMessage>();

        [Fact]
        public async Task Create_WhenUserTokenIsNotFound_ThrowNotFoundException()
        {
            // Arrange
            var handler = new CreateAppealMessageCommandHandler(userRepository, appealRepository,
                appealMessageRepository, logger, mapper, filesAdditionalToMessage);
            var command = new CreateAppealMessageWithUserCommand
            {
                AppealId = 1,
                Message = "",
                UserId = 1,
                Files = new List<FileDto> { File }
            };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
        [Fact]
        public async Task Create_WhenAppealIdAndUserTokenIsNotFound_ThrowNotFoundException()
        {
            // Arrange
            var handler = new CreateAppealMessageCommandHandler(userRepository, appealRepository, 
                appealMessageRepository, logger, mapper, filesAdditionalToMessage);
            userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(new User());
            var command = new CreateAppealMessageWithUserCommand
            {
                AppealId = 1,
                Message = "",
                UserId = 1,
                Files = new List<FileDto> { File }
            };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
        [Fact]
        public async Task Create_WhenAppealIdAndTokenIsFound_ReturnMessage()
        {
            // Arrange
            var command = new CreateAppealMessageWithUserCommand
            {
                AppealId = 1, Message = "test", UserId = 1,
                Files = new List<FileDto> { File }
            };
            var response = new AppealMessageResponse { AppealId = command.AppealId, Message = command.Message };
            mapper.Map<AppealMessageResponse>(null).ReturnsForAnyArgs(response);
            userRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>?>()).Returns(new User());
            appealRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<Appeal, bool>>?>()).Returns(new Appeal { Id = command.AppealId });

            var handler = new CreateAppealMessageCommandHandler(userRepository, appealRepository,
                appealMessageRepository, logger, mapper, filesAdditionalToMessage);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(command.AppealId, result.AppealId);
            Assert.Equal(command.Message, result.Message);
        }
    }
}
