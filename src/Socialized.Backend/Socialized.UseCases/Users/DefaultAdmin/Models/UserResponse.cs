namespace UseCases.Users.DefaultAdmin.Models
{
    public class UserResponse
    {
        public required long Id { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTimeOffset LastLoginAt { get; set; }
    }
}
