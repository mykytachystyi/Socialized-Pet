namespace Domain.Appeals.Repositories
{
    public interface IAppealQueryRepository
    {
        IEnumerable<Appeal> GetAppealsBy(string userToken, int since = 0, int count = 10, bool IsUserDeleted = false);
        IEnumerable<Appeal> GetAppealsBy(string userToken, int since = 0, int count = 10);
    }
}