namespace Domain.Users
{
    public class User : BaseEntity
    {
        public string TokenForUse { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public byte[] HashedPassword { get; set; } = null!;
        public byte[] HashedSalt { get; set; } = null!;
        public DateTime LastLoginAt { get; set; }
        public string HashForActivate { get; set; } = null!;
        public bool Activate { get; set; }
        public int? RecoveryCode { get; set; }
        public string RecoveryToken { get; set; } = null!;
        public virtual UserProfile Profile { get; set; } = null!;
    }
}