using UseCases.Appeals.Messages.Models;

namespace UseCases.Appeals.Models;

public record class AppealResponse
{
    public long Id { get; set; }
    public required string Subject { get; set; }
    public int State { get; set; }
    public DateTimeOffset LastActivity { get; set; }
}
