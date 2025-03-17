using Domain.Admins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(320);
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Role).IsRequired().HasMaxLength(100);
            builder.Property(e => e.HashedPassword);
            builder.Property(e => e.HashedSalt);
            builder.Property(e => e.TokenForStart).IsRequired().HasMaxLength(100);
            builder.Property(e => e.LastLoginAt).IsRequired();            
        }
    }
}