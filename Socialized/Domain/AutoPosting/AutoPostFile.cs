namespace Domain.AutoPosting
{
    public class AutoPostFile : BaseEntity
    {
        public long PostId { get; set; }
        public string Path { get; set; } = null!;
        public sbyte Order { get; set; }
        public bool Type { get; set; }
        public string MediaId { get; set; } = null!;
        public string VideoThumbnail { get; set; } = null!;
        public virtual AutoPost Post { get; set; } = null!;
    }
}