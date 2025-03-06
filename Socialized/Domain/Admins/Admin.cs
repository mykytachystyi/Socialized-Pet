namespace Domain.Admins
{
    public class Admin : BaseEntity
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string TokenForStart { get; set; } = null!;
        public DateTime LastLoginAt { get; set; }
        public int? RecoveryCode { get; set; }
    }
}
