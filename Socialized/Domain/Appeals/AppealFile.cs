namespace Domain.Appeals
{
    public class AppealFile : BaseEntity
    {
        public long MessageId { get; set; }
        public string RelativePath { get; set; } = null!;
        public virtual AppealMessage Message { get; set; }
    }
}