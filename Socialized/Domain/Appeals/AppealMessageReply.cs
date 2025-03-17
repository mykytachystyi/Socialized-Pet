namespace Domain.Appeals
{
    public class AppealMessageReply : BaseEntity
    {
        public long AppealMessageId { get; set; }
        public string Reply { get; set; } = null!;
        public DateTimeOffset UpdatedAt { get; set; }
        public virtual AppealMessage Message { get; set; } = null!;
    }
}
