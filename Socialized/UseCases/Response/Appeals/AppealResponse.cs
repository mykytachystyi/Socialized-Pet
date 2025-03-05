namespace UseCases.Response.Appeals
{
    public class AppealResponse
    {
        public long Id { get; set; }
        public required string Subject { get; set; }
        public int State { get; set; }
        public DateTimeOffset LastActivity { get; set; }
        public ICollection<AppealMessageResponse> Messages { get; set; }
    }
}
