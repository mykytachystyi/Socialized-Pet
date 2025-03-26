namespace Domain
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
    }
}
