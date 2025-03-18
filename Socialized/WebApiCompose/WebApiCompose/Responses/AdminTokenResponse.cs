namespace WebApiCompose.Responses;

public record class AdminTokenResponse
{
    public required string AdminToken { get; set; }
}