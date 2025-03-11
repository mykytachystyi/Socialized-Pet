namespace UseCases.Appeals.Files.Models
{
    public class AppealFileResponse
    {
        public long Id { get; set; }
        public long MessageId { get; set; }
        public required string RelativePath { get; set; }
    }
}
