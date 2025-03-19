using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Fullname).IsRequired();
            builder.Property(e => e.English).IsRequired();
            builder.Property(e => e.Location).IsRequired();
            builder.Property(e => e.LocationPrecise).IsRequired();
        }
    }
}