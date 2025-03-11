using AutoMapper;
using Domain.Appeals;
using Domain.Appeals.Repositories;
using NSubstitute;
using Serilog;
using UseCases.Appeals.Files.Models;
using UseCases.Appeals.Messages.CreateAppealMessage;
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
        private IAppealRepository appealRepository = Substitute.For<IAppealRepository>();
        private IAppealMessageRepository appealMessageRepository = Substitute.For<IAppealMessageRepository>();
        private IMapper mapper = Substitute.For<IMapper>();

        [Fact]
        public async Task Create_WhenAppealIdAndUserTokenIsNotFound_ThrowNotFoundException()
        {
            var handler = new CreateAppealMessageCommandHandler(appealRepository, appealMessageRepository, logger, mapper);
            var command = new CreateAppealMessageCommand
            {
                AppealId = 1,
                Message = "",
                UserToken = "",
                Files = new List<FileDto> { File }
            };

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
        [Fact]
        public async Task Create_WhenAppealIdAndTokenIsFound_ReturnMessage()
        {
            var command = new CreateAppealMessageCommand
            {
                AppealId = 1, Message = "test", UserToken = "",
                Files = new List<FileDto> { File }
            };
            appealRepository.GetBy(command.AppealId, command.UserToken)
                .Returns(new Appeal { Id = command.AppealId });

            var handler = new CreateAppealMessageCommandHandler(appealRepository, appealMessageRepository, logger, mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(command.AppealId, result.AppealId);
            Assert.Equal(command.Message, result.Message);
        }
    }
}
