using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class CultureConfiguration : IEntityTypeConfiguration<Culture>
    {
        public void Configure(EntityTypeBuilder<Culture> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Key).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Value).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        }
    }
}