using AutoMapper;
using Core.FileControl;
using Domain.Appeals;
using Domain.Appeals.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Appeals.Files.CreateAppealMessageFile;
using UseCases.Appeals.Files.Models;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals
{
    public class CreateAppealFileTests
    {
        private int messageId = 1;
        private IMapper mapper = Substitute.For<IMapper>();
        private ILogger logger = Substitute.For<ILogger>();
        private IFileManager fileManager = Substitute.For<IFileManager>();
        private IAppealFileRepository appealFileRepository = Substitute.For<IAppealFileRepository>();

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

        [Fact]
        public async Task Create_WhenFilesIsEmpty_ReturnEmptyCollection()
        {
            appealFileRepository.GetById(messageId).Returns(new AppealMessage());
            var handler = new CreateAppealMessageFileCommandHandler(fileManager, logger, appealFileRepository, mapper);
            var command = new CreateAppealMessageFileCommand
            {
                MessageId = messageId,
                Upload = new List<FileDto> { }
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Empty(result);
        }
        [Fact]
        public async Task Create_WhenFilesIsExist_ReturnFilesCollection()
        {
            appealFileRepository.GetById(messageId).Returns(new AppealMessage());
            var appealFile = new AppealFileResponse { Id = 1, MessageId = 1, RelativePath = "" };
            mapper.Map<IEnumerable<AppealFileResponse>>(null).ReturnsForAnyArgs(new List<AppealFileResponse> { appealFile });

            var handler = new CreateAppealMessageFileCommandHandler(fileManager, logger, appealFileRepository, mapper);

            var command = new CreateAppealMessageFileCommand
            {
                MessageId = messageId,
                Upload = new List<FileDto> { File }
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Single(result);
        }
        [Fact]
        public async Task Create_WhenFilesIsExist_ThrowNotFoundException()
        {
            appealFileRepository.GetById(messageId).ReturnsNull();

            var handler = new CreateAppealMessageFileCommandHandler(fileManager, logger, appealFileRepository, mapper);


            var command = new CreateAppealMessageFileCommand
            {
                MessageId = messageId,
                Upload = new List<FileDto> { File }
            };

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}