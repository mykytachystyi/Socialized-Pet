using Domain.InstagramAccounts;

namespace Domain.AutoPosting
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Links = new HashSet<AutoPost>();
        }

        public long AccountId { get; set; }
        public string Name { get; set; } = null!;
        public string Color { get; set; } = null!;
        public bool Deleted { get; set; }
        public virtual IGAccount Account { get; set; } = null!;
        public virtual ICollection<AutoPost> Links { get; set; }
    }
}