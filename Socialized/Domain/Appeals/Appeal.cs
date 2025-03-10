using Domain.Users;

namespace Domain.Appeals
{
    public class Appeal : BaseEntity
    {
        public Appeal()
        {
            Messages = new HashSet<AppealMessage>();
        }
        public long UserId { get; set; }
        public string Subject { get; set; } = null!;
        public int State { get; set; }
        public DateTimeOffset LastActivity { get; set; }
        public virtual User User { get; set; } = null!;
        public ICollection<AppealMessage> Messages { get; set; }
    }
}