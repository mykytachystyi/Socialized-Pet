using Domain.Users;
using Domain.Appeals;
using Microsoft.EntityFrameworkCore;
using Infrastructure.EntityTypeConfiguration;
using Core.Providers.Hmac;
using Domain.Enums;

namespace Infrastructure
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<AppealFile> AppealFiles { get; set; }
        public virtual DbSet<AppealMessage> AppealMessages { get; set; }
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
            modelBuilder.ApplyConfiguration(new AppealConfiguration());
            modelBuilder.ApplyConfiguration(new AppealFileConfiguration());
            modelBuilder.ApplyConfiguration(new AppealMessageConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CultureConfiguration());

            var provider = new HmacSha256Provider();
            var hashedPassword = provider.HashPassword("Pass1234!");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@example.com",
                    FirstName = "Main",
                    LastName = "Hero",
                    Role = (int)IdentityRole.DefaultAdmin,
                    HashedPassword = hashedPassword.Hash,
                    HashedSalt = hashedPassword.Salt,
                    CreatedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow,
                    Activate = true,
                    HashForActivate = Guid.NewGuid().ToString(),
                    RecoveryCode = 1234,
                    RecoveryToken = Guid.NewGuid().ToString(),
                },
                new User
                {
                    Id = 2,
                    Email = "user@example.com",
                    FirstName = "Second",
                    LastName = "Hero",
                    Role = (int)IdentityRole.DefaultUser,
                    HashedPassword = hashedPassword.Hash,
                    HashedSalt = hashedPassword.Salt,
                    CreatedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow,
                    Activate = true,
                    HashForActivate = Guid.NewGuid().ToString(),
                    RecoveryCode = 1234,
                    RecoveryToken = Guid.NewGuid().ToString(),
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
