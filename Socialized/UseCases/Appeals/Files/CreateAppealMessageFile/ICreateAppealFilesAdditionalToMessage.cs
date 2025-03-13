using Domain.Appeals;
using UseCases.Appeals.Files.Models;

namespace UseCases.Appeals.Files.CreateAppealMessageFile
{
    public interface ICreateAppealFilesAdditionalToMessage
    {
        HashSet<AppealFile> Create(ICollection<FileDto> upload, AppealMessage message);
    }
}
