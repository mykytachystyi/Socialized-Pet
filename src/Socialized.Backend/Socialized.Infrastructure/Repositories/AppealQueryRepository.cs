using Domain.Appeals;
using Domain.Appeals.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AppealQueryRepository : IAppealQueryRepository
    {
        private AppDbContext _context;
        public AppealQueryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Appeal>> GetAppealsByAsync(long userId, int since = 0, int count = 10, bool IsUserDeleted = false)
        {
            return await _context.Appeals
                .Include(appeal => appeal.Messages)
                    .ThenInclude(message => message.Files)
                .Include(appeal => appeal.User)
                .Where(appeal => appeal.User.Id == userId
                    && !appeal.User.IsDeleted == IsUserDeleted)
                .OrderBy(appeal => appeal.State)
                .ThenByDescending(appeal => appeal.CreatedAt)
                .Skip(since * count)
                .Take(count)
                .ToArrayAsync();
        }
        public async Task<IEnumerable<Appeal>> GetAppealsByAsync(long userId, int since = 0, int count = 10)
        {
            return await _context.Appeals
                .Include(appeal => appeal.Messages)
                    .ThenInclude(message => message.Files)
                .Include(appeal => appeal.User)
                .Where(appeal => appeal.User.Id == userId
                    && !appeal.User.IsDeleted)
                .OrderBy(appeal => appeal.State)
                .ThenByDescending(appeal => appeal.CreatedAt)
                .Skip(since * count)
                .Take(count)
                .ToArrayAsync();
        }
    }
}