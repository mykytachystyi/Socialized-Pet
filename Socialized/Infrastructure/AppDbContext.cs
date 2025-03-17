using Core;
using Domain.Users;
using Domain.Admins;
using Domain.Appeals;
using Microsoft.EntityFrameworkCore;
using Infrastructure.EntityTypeConfiguration;
using Core.Providers;
using Core.Providers.Hmac;
using Core.Providers.Rand;

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

            var provider = new HmacSha256Provider();
            var hashedPassword = provider.HashPassword("Pass1234!");

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Email = "user@example.com",
                    FirstName = "default",
                    LastName = "default",
                    Role = "default",
                    HashedPassword = hashedPassword.Hash,
                    HashedSalt = hashedPassword.Salt,
                    CreatedAt = DateTime.Now,
                    TokenForStart = new Randomizer().CreateHash(10)
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
