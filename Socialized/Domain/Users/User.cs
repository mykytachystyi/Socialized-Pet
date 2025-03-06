using Domain.InstagramAccounts;
using Domain.Packages;
using Domain.Appeals;

namespace Domain.Users
{
    public class User : BaseEntity
    {
        public User()
        {
            IGAccounts = new HashSet<IGAccount>();
        }

        public string TokenForUse { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime LastLoginAt { get; set; }
        public string HashForActivate { get; set; } = null!;
        public bool Activate { get; set; }
        public int? RecoveryCode { get; set; }
        public string RecoveryToken { get; set; } = null!;
        public virtual ServiceAccess Access { get; set; } = null!;
        public virtual UserProfile Profile { get; set; } = null!;
        public virtual ICollection<IGAccount> IGAccounts { get; set; }
        public virtual ICollection<Appeal> Appeals { get; set; }
    }
}