namespace Domain.Appeals.Repositories
{
    public interface IAppealQueryRepository
    {
        Task<IEnumerable<Appeal>> GetAppealsByAsync(long userId, int since = 0, int count = 10, bool IsUserDeleted = false);
        Task<IEnumerable<Appeal>> GetAppealsByAsync(long userId, int since = 0, int count = 10);
    }
}