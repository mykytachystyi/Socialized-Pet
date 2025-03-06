namespace Domain.GettingSubscribes
{
    public class TaskData : BaseEntity
    {
        public long TaskId { get; set; }
        public string Names { get; set; } = null!;
        public double? Longitute { get; set; }
        public double? Latitute { get; set; }
        public string? Comment { get; set; }
        public bool Stopped { get; set; }
        public int NextPage { get; set; }
        public virtual TaskGS Task { get; set; } = null!;
    }
}
