using Core.FileControl.CurrentFileSystem;
using Domain.Appeals;
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
            // Arrange
            messageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>())
                .Returns(new AppealMessage());
            var handler = new CreateAppealMessageFileCommandHandler(fileManager, logger, messageRepository,
                appealFileRepository);
            var command = new CreateAppealMessageFileCommand
            {
                MessageId = messageId,
                Upload = new List<FileDto> { }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
        [Fact]
        public async Task Create_WhenFilesIsExist_ReturnFilesCollection()
        {
            // Arrange
            messageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>())
                .Returns(new AppealMessage());
            var appealFile = new AppealFileResponse { Id = 1, MessageId = 1, RelativePath = "" };
            var handler = new CreateAppealMessageFileCommandHandler(fileManager, logger, messageRepository,
                appealFileRepository);
            var command = new CreateAppealMessageFileCommand
            {
                MessageId = messageId,
                Upload = new List<FileDto> { File }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Single(result);
        }
        [Fact]
        public async Task Create_WhenFilesIsExist_ThrowNotFoundException()
        {
            // Arrange
            messageRepository.FirstOrDefaultAsync(Arg.Any<Expression<Func<AppealMessage, bool>>?>())
                .ReturnsNull();
            var handler = new CreateAppealMessageFileCommandHandler(fileManager, logger, messageRepository,
                appealFileRepository);

            var command = new CreateAppealMessageFileCommand
            {
                MessageId = messageId,
                Upload = new List<FileDto> { File }
            };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}