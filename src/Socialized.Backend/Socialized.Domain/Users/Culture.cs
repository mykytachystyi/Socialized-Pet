namespace Domain.Users
{
    public class Culture : BaseEntity
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}