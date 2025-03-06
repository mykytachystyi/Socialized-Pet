namespace Domain.GettingSubscribes
{
    public class WordFilter : BaseEntity
    {
        public long FilterId { get; set; }
        public string Value { get; set; } = null!;
        public bool Use { get; set; }
        public virtual TaskFilter Filter { get; set; } = null!;
    }
}