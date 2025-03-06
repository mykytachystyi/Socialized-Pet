using Domain.Users;
using Domain.AutoPosting;
using Domain.GettingSubscribes;

namespace Domain.InstagramAccounts
{
    public class IGAccount : BaseEntity
    {
        public long UserId { get; set; }
        public string Username { get; set; } = null!;
        public virtual BusinessAccount Business { get; set; } = null!;
        public virtual AccountProfile Profile { get; set; } = null!;
        public virtual SessionState State { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<TaskGS> Tasks { get; set; }
        public virtual ICollection<AutoPost> AutoPosts { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public IGAccount()
        {
            Categories = new HashSet<Category>();
            Tasks = new HashSet<TaskGS>();
            AutoPosts = new HashSet<AutoPost>();
        }
    }
}
