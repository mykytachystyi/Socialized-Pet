using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.TokenForUse).IsRequired();
            builder.Property(e => e.Email).IsRequired().HasMaxLength(320);
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Password).IsRequired().HasMaxLength(100);
            builder.Property(e => e.LastLoginAt).IsRequired();
            builder.Property(e => e.HashForActivate).IsRequired();
            builder.Property(e => e.Activate).IsRequired();
            builder.Property(e => e.RecoveryCode);
            builder.Property(e => e.RecoveryToken).IsRequired(false);
        }
    }
}