using AutoMapper;
using Core.FileControl;
using Domain.Appeals;
using Domain.Appeals.Repositories;
using MediatR;
using Serilog;
using UseCases.Appeals.Files.Models;
using UseCases.Base;
using UseCases.Exceptions;

namespace UseCases.Appeals.Files.CreateAppealMessageFile;

public class CreateAppealMessageFileCommandHandler (
    FileManager fileManager,
    ILogger logger,
    IAppealFileRepository AppealFilesRepository,
    IMapper mapper
    ) : IRequestHandler<CreateAppealMessageFileCommand, IEnumerable<AppealFileResponse>>
{
    public async Task<IEnumerable<AppealFileResponse>> Handle(
        CreateAppealMessageFileCommand request, 
        CancellationToken cancellationToken)
    {
        logger.Information("Початок обробки команди створення файлів для повідомлення в зверненні.");

        var message = AppealFilesRepository.GetById(request.MessageId);

        if (message == null)
        {
            throw new NotFoundException($"Сервер не визначив звернення по id={request.MessageId}.");
        }
        var files = Create(request.Upload, message);

        return mapper.Map<IEnumerable<AppealFileResponse>>(files);
    }
    public HashSet<AppealFile> Create(ICollection<FileDto> upload, AppealMessage message)
    {
        logger.Information("Початок створення файлів для повідомлення в зверненні.");
        var files = new HashSet<AppealFile>();
        if (upload != null)
        {
            foreach (var file in upload)
            {
                var savedFile = fileManager.SaveFileAsync(file.OpenReadStream(), "AppealFiles");
                var saved = new AppealFile
                {
                    MessageId = message.Id,
                    RelativePath = savedFile.Result,
                    Message = message
                };
                files.Add(saved);
            }
            AppealFilesRepository.Create(files);
            logger.Information("Були створені файли для повідомлення в зверненні.");
        }
        else
        {
            logger.Information("Не було завантажено файлів для повідомлення в зверненні.");
        }    
        return files;
    }
}