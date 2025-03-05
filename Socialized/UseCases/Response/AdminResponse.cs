namespace UseCases.Response
{
    public class AdminResponse
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Role { get; set; }
        public DateTime LastLoginAt { get; set; }
    }
}
