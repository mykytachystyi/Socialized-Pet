using Core;
using Domain.Users;
using Domain.Admins;
using Domain.Appeals;
using Microsoft.EntityFrameworkCore;
using Infrastructure.EntityTypeConfiguration;

namespace Infrastructure
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AppealFile> AppealFiles { get; set; }
        public virtual DbSet<AppealMessage> AppealMessages { get; set; }
        public virtual DbSet<AppealMessageReply> AppealReplies { get; set; }
        public virtual DbSet<Appeal> Appeals { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Culture> Cultures { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
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
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CultureConfiguration());

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Email = "user@example.com",
                    FirstName = "default",
                    LastName = "default",
                    Role = "default",
                    Password = new ProfileCondition().HashPassword("Pass1234!"),
                    CreatedAt = DateTime.Now,
                    TokenForStart = new ProfileCondition().CreateHash(10)
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
