using Microsoft.EntityFrameworkCore;
using Domain.Users;
using Domain.Admins;
using Domain.AutoPosting;
using Domain.GettingSubscribes;
using Domain.Packages;
using Domain.InstagramAccounts;
using Core;
using Domain.Appeals;
using Infrastructure.EntityTypeConfiguration;

namespace Infrastructure
{
    public partial class Context : DbContext
    {
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AppealFile> AppealFiles { get; set; }
        public virtual DbSet<AppealMessage> AppealMessages { get; set; }
        public virtual DbSet<AppealMessageReply> AppealReplies { get; set; }
        public virtual DbSet<Appeal> Appeals { get; set; }
        public virtual DbSet<AutoPostFile> AutoPostFiles { get; set; }
        public virtual DbSet<AutoPost> AutoPosts { get; set; }
        public virtual DbSet<BusinessAccount> BusinessAccounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Culture> Cultures { get; set; }
        public virtual DbSet<DiscountPackage> DiscountPackages { get; set; }
        public virtual DbSet<IGAccount> IGAccounts { get; set; }
        public virtual DbSet<PackageAccess> PackageAccess { get; set; }
        public virtual DbSet<ServiceAccess> ServiceAccess { get; set; }
        public virtual DbSet<AccountProfile> SessionProfiles { get; set; }
        public virtual DbSet<SessionState> States { get; set; }
        public virtual DbSet<TaskData> TaskData { get; set; }
        public virtual DbSet<TaskFilter> TaskFilters { get; set; }
        public virtual DbSet<TaskGS> TaskGS { get; set; }
        public virtual DbSet<TaskOption> TaskOptions { get; set; }
        public virtual DbSet<TimeAction> TimeAction { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new AppealConfiguration());
            modelBuilder.ApplyConfiguration(new AppealFileConfiguration());
            modelBuilder.ApplyConfiguration(new AppealMessageConfiguration());
            modelBuilder.ApplyConfiguration(new AppealMessageReplyConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            


            modelBuilder.Entity<AutoPost>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountId).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Executed).IsRequired();
                entity.Property(e => e.Deleted).IsRequired();
                entity.Property(e => e.Stopped).IsRequired();
                entity.Property(e => e.AutoDelete).IsRequired();
                entity.Property(e => e.AutoDeleted).IsRequired();
                entity.Property(e => e.ExecuteAt).IsRequired();
                entity.Property(e => e.DeleteAfter).IsRequired();
                entity.Property(e => e.Location).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Comment).IsRequired();
                entity.Property(e => e.TimeZone).IsRequired();
                entity.Property(e => e.CategoryId).IsRequired();

                entity.HasOne(e => e.Account)
                      .WithMany(a => a.AutoPosts)
                      .HasForeignKey(e => e.AccountId);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Links)
                      .HasForeignKey(e => e.CategoryId);

                entity.HasMany(e => e.Files)
                      .WithOne(f => f.Post)
                      .HasForeignKey(f => f.PostId);
            });
            modelBuilder.Entity<AutoPostFile>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.PostId).IsRequired();
                entity.Property(e => e.Path).IsRequired();
                entity.Property(e => e.Order).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.MediaId).IsRequired();
                entity.Property(e => e.VideoThumbnail).IsRequired();

                entity.HasOne(e => e.Post)
                      .WithMany(p => p.Files)
                      .HasForeignKey(e => e.PostId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountId).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Color).IsRequired();
                entity.Property(e => e.Deleted).IsRequired();

                entity.HasOne(e => e.Account)
                      .WithMany(a => a.Categories)
                      .HasForeignKey(e => e.AccountId);

                entity.HasMany(e => e.Links)
                      .WithOne(l => l.Category)
                      .HasForeignKey(l => l.CategoryId);
            });

            modelBuilder.Entity<TaskData>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TaskId).IsRequired();
                entity.Property(e => e.Names).IsRequired();
                entity.Property(e => e.Longitute);
                entity.Property(e => e.Latitute);
                entity.Property(e => e.Comment).IsRequired();
                entity.Property(e => e.Stopped).IsRequired();
                entity.Property(e => e.NextPage).IsRequired();

                entity.HasOne(e => e.Task)
                      .WithMany(t => t.Data)
                      .HasForeignKey(e => e.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<TaskFilter>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TaskId).IsRequired();
                entity.Property(e => e.RangeSubscribersFrom).IsRequired();
                entity.Property(e => e.RangeSubscribersTo).IsRequired();
                entity.Property(e => e.RangeFollowingFrom).IsRequired();
                entity.Property(e => e.RangeFollowingTo).IsRequired();
                entity.Property(e => e.PublicationCount).IsRequired();
                entity.Property(e => e.LatestPublicationNoYounger).IsRequired();
                entity.Property(e => e.WithoutProfilePhoto).IsRequired();
                entity.Property(e => e.WithProfileUrl).IsRequired();
                entity.Property(e => e.English).IsRequired();
                entity.Property(e => e.Ukrainian).IsRequired();
                entity.Property(e => e.Russian).IsRequired();
                entity.Property(e => e.Arabian).IsRequired();

                entity.HasOne(e => e.Task)
                      .WithOne(t => t.Filter)
                      .HasForeignKey<TaskFilter>(e => e.TaskId);

                entity.HasMany(e => e.words)
                      .WithOne(w => w.Filter)
                      .HasForeignKey(w => w.FilterId);
            });
            modelBuilder.Entity<TaskGS>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountId).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Subtype).IsRequired();
                entity.Property(e => e.LastDoneAt).IsRequired();
                entity.Property(e => e.Updated).IsRequired();
                entity.Property(e => e.Running).IsRequired();
                entity.Property(e => e.Stopped).IsRequired();
                entity.Property(e => e.NextTaskData).IsRequired();
                entity.Property(e => e.Deleted).IsRequired();

                entity.HasOne(e => e.Account)
                      .WithMany(a => a.Tasks)
                      .HasForeignKey(e => e.AccountId);

                entity.HasOne(e => e.Filter)
                      .WithOne(f => f.Task)
                      .HasForeignKey<TaskFilter>(f => f.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Option)
                      .WithOne(o => o.Task)
                      .HasForeignKey<TaskOption>(o => o.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Data)
                      .WithOne(d => d.Task)
                      .HasForeignKey(d => d.TaskId);
            });
            modelBuilder.Entity<TaskOption>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TaskId).IsRequired();
                entity.Property(e => e.DontFollowOnPrivate).IsRequired();
                entity.Property(e => e.WatchStories).IsRequired();
                entity.Property(e => e.LikeUsersPost).IsRequired();
                entity.Property(e => e.AutoUnfollow).IsRequired();
                entity.Property(e => e.UnfollowNonReciprocal).IsRequired();
                entity.Property(e => e.NextUnlocking).IsRequired();
                entity.Property(e => e.LikesOnUser).IsRequired();

                entity.HasOne(e => e.Task)
                      .WithOne(t => t.Option)
                      .HasForeignKey<TaskOption>(e => e.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WordFilter>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FilterId).IsRequired();
                entity.Property(e => e.Value).IsRequired();
                entity.Property(e => e.Use).IsRequired();

                entity.HasOne(e => e.Filter)
                      .WithMany(f => f.words)
                      .HasForeignKey(e => e.FilterId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<AccountProfile>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountId).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.PostsCount).IsRequired();
                entity.Property(e => e.FollowingCount).IsRequired();
                entity.Property(e => e.SubscribersCount).IsRequired();
                entity.Property(e => e.AvatarUrl).IsRequired();
                entity.Property(e => e.SubscribersGS).IsRequired();
                entity.Property(e => e.SubscribersTodayGS).IsRequired();
                entity.Property(e => e.ConversionGS).IsRequired();

                entity.HasOne(e => e.Account)
                      .WithOne(a => a.Profile)
                      .HasForeignKey<AccountProfile>(e => e.AccountId);
            });
            modelBuilder.Entity<BusinessAccount>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountId).IsRequired();
                entity.Property(e => e.AccessToken).IsRequired();
                entity.Property(e => e.ProfilePicture).IsRequired();
                entity.Property(e => e.AccountUsername).IsRequired();
                entity.Property(e => e.LongLiveAccessToken).IsRequired();
                entity.Property(e => e.FacebookId).IsRequired();
                entity.Property(e => e.BusinessAccountId).IsRequired();
                entity.Property(e => e.FollowersCount).IsRequired();
                entity.Property(e => e.MediaCount).IsRequired();
                entity.Property(e => e.LongTokenExpiresIn).IsRequired();
                entity.Property(e => e.TokenCreated).IsRequired();
                entity.Property(e => e.Received).IsRequired();
                entity.Property(e => e.StartProcess).IsRequired();
                entity.Property(e => e.StartedProcess).IsRequired();

                entity.HasOne(e => e.Account)
                      .WithOne(a => a.Business)
                      .HasForeignKey<BusinessAccount>(e => e.AccountId);
            });
            modelBuilder.Entity<IGAccount>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Username).IsRequired();

                entity.HasOne(e => e.Business)
                      .WithOne(b => b.Account)
                      .HasForeignKey<BusinessAccount>(b => b.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Profile)
                      .WithOne(p => p.Account)
                      .HasForeignKey<AccountProfile>(p => p.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.State)
                      .WithOne(s => s.Account)
                      .HasForeignKey<SessionState>(s => s.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.IGAccounts)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Tasks)
                      .WithOne(t => t.Account)
                      .HasForeignKey(t => t.AccountId);

                entity.HasMany(e => e.AutoPosts)
                      .WithOne(a => a.Account)
                      .HasForeignKey(a => a.AccountId);

                entity.HasMany(e => e.Categories)
                      .WithOne(c => c.Account)
                      .HasForeignKey(c => c.AccountId);
            });
            modelBuilder.Entity<SessionState>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountId).IsRequired();
                entity.Property(e => e.SessionSave).IsRequired();
                entity.Property(e => e.Usable).IsRequired();
                entity.Property(e => e.Challenger).IsRequired();
                entity.Property(e => e.Relogin).IsRequired();
                entity.Property(e => e.Spammed).IsRequired();
                entity.Property(e => e.SpammedStarted).IsRequired();
                entity.Property(e => e.SpammedEnd).IsRequired();

                entity.HasOne(e => e.Account)
                      .WithOne(a => a.State)
                      .HasForeignKey<SessionState>(e => e.AccountId);

                entity.HasOne(e => e.TimeAction)
                      .WithOne(ta => ta.SessionState)
                      .HasForeignKey<TimeAction>(ta => ta.SessionId);
            });
            modelBuilder.Entity<TimeAction>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.SessionId).IsRequired();
                entity.Property(e => e.SessionOld).IsRequired();
                entity.Property(e => e.FollowCount).IsRequired();
                entity.Property(e => e.UnfollowCount).IsRequired();
                entity.Property(e => e.LikeCount).IsRequired();
                entity.Property(e => e.CommentCount).IsRequired();
                entity.Property(e => e.MentionsCount).IsRequired();
                entity.Property(e => e.BlockCount).IsRequired();
                entity.Property(e => e.PublicationCount).IsRequired();
                entity.Property(e => e.MessageDirectCount).IsRequired();
                entity.Property(e => e.WatchingStoriesCount).IsRequired();
                entity.Property(e => e.FollowLastAt).IsRequired();
                entity.Property(e => e.UnfollowLastAt).IsRequired();
                entity.Property(e => e.LikeLastAt).IsRequired();
                entity.Property(e => e.CommentLastAt).IsRequired();
                entity.Property(e => e.MentionsLastAt).IsRequired();
                entity.Property(e => e.BlockLastAt).IsRequired();
                entity.Property(e => e.PublicationLastAt).IsRequired();
                entity.Property(e => e.MessageDirectLastAt).IsRequired();
                entity.Property(e => e.WatchingStoriesLastAt).IsRequired();

                entity.HasOne(e => e.SessionState)
                      .WithOne(a => a.TimeAction)
                      .HasForeignKey<TimeAction>(e => e.SessionId);
            });

            modelBuilder.Entity<DiscountPackage>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Percent).IsRequired();
                entity.Property(e => e.Day).IsRequired();
                entity.Property(e => e.Month).IsRequired();
            });

            modelBuilder.Entity<PackageAccess>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.IGAccounts).IsRequired();
                entity.Property(e => e.Posts).IsRequired();
                entity.Property(e => e.Stories).IsRequired();
                entity.Property(e => e.AnalyticsDays).IsRequired();
            });

            modelBuilder.Entity<ServiceAccess>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Available).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Paid).IsRequired();
                entity.Property(e => e.PaidAt).IsRequired();
                entity.Property(e => e.DisableAt).IsRequired();

                entity.HasOne(e => e.User)
                      .WithOne(u => u.Access)
                      .HasForeignKey<ServiceAccess>(e => e.UserId);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasColumnType("varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci");

                entity.Property(e => e.Fullname)
                      .IsRequired()
                      .HasColumnType("varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci");

                entity.Property(e => e.English)
                      .IsRequired()
                      .HasColumnType("varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci");

                entity.Property(e => e.Location)
                      .IsRequired()
                      .HasColumnType("varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci");

                entity.Property(e => e.LocationPrecise)
                      .IsRequired()
                      .HasColumnType("varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci");
            });

            modelBuilder.Entity<Culture>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Key)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Value)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Culture>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Key)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Value)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });


            var profileCondition = new ProfileCondition();
            modelBuilder.Entity<PackageAccess>().HasData(
                new PackageAccess 
                { 
                    Id = 1, 
                    Name = "default", 
                    Price = 250, 
                    IGAccounts = 3, 
                    Posts = 10, 
                    Stories = 10, 
                    AnalyticsDays = 30 
                }
            );
            modelBuilder.Entity<DiscountPackage>().HasData(
                new DiscountPackage
                {
                    Id = 1,
                    Percent = 50,
                    Day = 10,
                    Month = 1,
                }
            );
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Email = "user@example.com",
                    FirstName = "default",
                    LastName = "default",
                    Role = "default",
                    Password = profileCondition.HashPassword("Pass1234!"),
                    CreatedAt = DateTime.Now,
                    TokenForStart = ""
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
