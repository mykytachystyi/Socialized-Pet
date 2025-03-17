using AutoMapper;
using Core.FileControl.CurrentFileSystem;
using Domain.Appeals;
using Domain.Users;
using Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using System.Linq.Expressions;
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
        private IRepository<AppealFile> appealFileRepository = Substitute.For<IRepository<AppealFile>>();
        private IRepository<AppealMessage> messageRepository = Substitute.For<IRepository<AppealMessage>>();
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
            messageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>())
                .Returns(new AppealMessage());
            var handler = new CreateAppealMessageFileCommandHandler(fileManager, logger, messageRepository,
                appealFileRepository, mapper);
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
            messageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>())
                .Returns(new AppealMessage());
            var appealFile = new AppealFileResponse { Id = 1, MessageId = 1, RelativePath = "" };
            mapper.Map<IEnumerable<AppealFileResponse>>(null).ReturnsForAnyArgs(new List<AppealFileResponse> { appealFile });
            var handler = new CreateAppealMessageFileCommandHandler(fileManager, logger, messageRepository,
                appealFileRepository, mapper);
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
            messageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>())
                .ReturnsNull();
            var handler = new CreateAppealMessageFileCommandHandler(fileManager, logger, messageRepository,
                appealFileRepository, mapper);

            var command = new CreateAppealMessageFileCommand
            {
                MessageId = messageId,
                Upload = new List<FileDto> { File }
            };

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}