namespace Domain.Users
{
    public class Country : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Fullname { get; set; } = null!;
		public string English { get; set; } = null!;
		public string Location { get; set; } = null!;
		public string LocationPrecise { get; set; } = null!;
    }
}