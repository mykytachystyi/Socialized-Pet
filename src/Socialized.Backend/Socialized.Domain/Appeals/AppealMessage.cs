namespace Domain.Appeals
{
    public class AppealMessage : BaseEntity
    {
        public AppealMessage()
        {
            Files = new HashSet<AppealFile>();
        }
        public long AppealId { get; set; }
        public string Message { get; set; } = null!;
        public DateTimeOffset UpdatedAt { get; set; }
        public virtual Appeal Appeal { get; set; } = null!;
        public virtual ICollection<AppealFile> Files { get; set; }
    }
}