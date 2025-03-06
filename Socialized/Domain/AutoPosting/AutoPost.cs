using Domain.InstagramAccounts;

namespace Domain.AutoPosting
{
    public class AutoPost : BaseEntity
    {
        public AutoPost()
        {
            Files = new HashSet<AutoPostFile>();
        }
        public long AccountId { get; set; }
        public bool Type { get; set; }
        public bool Executed { get; set; }
        public bool Deleted { get; set; }
        public bool Stopped { get; set; }
        public bool AutoDelete { get; set; }
        public bool AutoDeleted { get; set; }
        public DateTime ExecuteAt { get; set; }
        public DateTime DeleteAfter { get; set; }
        public string Location { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int TimeZone { get; set; }
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        public virtual IGAccount Account { get; set; } = null!;
        public virtual ICollection<AutoPostFile> Files { get; set; }
    }
}