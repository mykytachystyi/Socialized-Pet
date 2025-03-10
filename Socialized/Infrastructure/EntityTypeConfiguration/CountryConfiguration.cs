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

            builder.Property(e => e.Name).IsRequired()
                .HasColumnType("varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci");
            builder.Property(e => e.Fullname).IsRequired()
                .HasColumnType("varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci");
            builder.Property(e => e.English).IsRequired()
                .HasColumnType("varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci");
            builder.Property(e => e.Location).IsRequired()
                .HasColumnType("varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci");
            builder.Property(e => e.LocationPrecise).IsRequired()
                .HasColumnType("varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci");
        }
    }
}