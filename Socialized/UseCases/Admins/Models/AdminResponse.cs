namespace UseCases.Admins.Models
{
    public class AdminResponse
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Role { get; set; }
        public DateTimeOffset LastLoginAt { get; set; }
    }
}
