namespace Domain.InstagramAccounts
{
    public class BusinessAccount : BaseEntity
    {
        public long AccountId { get; set; }
        public string AccessToken { get; set; } = null!;
        public string ProfilePicture { get; set; } = null!;
        public string AccountUsername { get; set; } = null!;
        public string LongLiveAccessToken { get; set; } = null!;
        public string FacebookId { get; set; } = null!;
        public string BusinessAccountId { get; set; } = null!;
        public long FollowersCount { get; set; }
        public int MediaCount { get; set; }
        public DateTime LongTokenExpiresIn { get; set; }
        public DateTime TokenCreated { get; set; }
        public bool Received { get; set; }
        public bool StartProcess { get; set; }
        public DateTime StartedProcess { get; set; }
        public virtual IGAccount Account { get; set; } = null!;
    }
}