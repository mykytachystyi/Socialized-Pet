namespace UseCases.Response.Appeals
{
    public class AppealMessageResponse
    {
        public long Id { get; set; }
        public long AppealId { get; set; }
        public string Message { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public virtual ICollection<AppealFileResponse> Files { get; set; }
        public virtual ICollection<AppealReplyResponse> AppealMessageReplies { get; set; }
    }
}