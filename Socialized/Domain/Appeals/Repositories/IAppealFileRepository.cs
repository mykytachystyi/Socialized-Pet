namespace Domain.Appeals.Repositories
{
    public interface IAppealFileRepository
    {
        ICollection<AppealFile> Create(ICollection<AppealFile> files);
        AppealMessage? GetById(long messageId);
    }
}
