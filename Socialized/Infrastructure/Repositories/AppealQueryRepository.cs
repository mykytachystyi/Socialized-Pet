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
        public IEnumerable<Appeal> GetAppealsBy(string userToken, int since = 0, int count = 10, bool IsUserDeleted = false)
        {
            return _context.Appeals
                .Include(appeal => appeal.Messages)
                    .ThenInclude(message => message.Files)
                .Include(appeal => appeal.Messages)
                    .ThenInclude(message => message.AppealMessageReplies)
                .Include(appeal => appeal.User)
                .Where(appeal => appeal.User.TokenForUse == userToken
                    && !appeal.User.IsDeleted == IsUserDeleted)
                .OrderBy(appeal => appeal.State)
                .ThenByDescending(appeal => appeal.CreatedAt)
                .Skip(since * count)
                .Take(count)
                .ToArray();
        }
        public IEnumerable<Appeal> GetAppealsBy(string userToken, int since = 0, int count = 10)
        {
            return _context.Appeals
                .Include(appeal => appeal.Messages)
                    .ThenInclude(message => message.Files)
                .Include(appeal => appeal.Messages)
                    .ThenInclude(message => message.AppealMessageReplies)
                .Include(appeal => appeal.User)
                .Where(appeal => appeal.User.TokenForUse == userToken
                    && !appeal.User.IsDeleted)
                .OrderBy(appeal => appeal.State)
                .ThenByDescending(appeal => appeal.CreatedAt)
                .Skip(since * count)
                .Take(count)
                .ToArray();
        }
    }
}