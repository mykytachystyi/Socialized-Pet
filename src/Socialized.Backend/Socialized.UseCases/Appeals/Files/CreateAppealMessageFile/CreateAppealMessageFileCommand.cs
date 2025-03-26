using MediatR;
using UseCases.Appeals.Files.Models;

namespace UseCases.Appeals.Files.CreateAppealMessageFile;

public class CreateAppealMessageFileCommand : IRequest<IEnumerable<AppealFileResponse>>
{ 
    public long UserId { get; set; }
    public long MessageId { get; set; }
    public ICollection<FileDto>? Upload { get; set; }
}