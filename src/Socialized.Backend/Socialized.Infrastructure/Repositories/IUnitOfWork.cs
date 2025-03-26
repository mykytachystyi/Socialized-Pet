namespace Infrastructure.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>() where T : class;
    TRepository GetEntityRepository<TRepository>() where TRepository : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}