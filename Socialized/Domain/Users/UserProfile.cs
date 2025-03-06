namespace Domain.Users
{
    public class UserProfile : BaseEntity
    {
        public long UserId { get; set; }
        public string CountryName { get; set; } = null!;
        public long TimeZone { get; set; }
        public virtual User user { get; set; } = null!;
    }
}