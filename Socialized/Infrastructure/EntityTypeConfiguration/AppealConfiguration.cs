using Domain.Appeals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class AppealConfiguration : IEntityTypeConfiguration<Appeal>
    {
        public void Configure(EntityTypeBuilder<Appeal> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.Subject).IsRequired();
            builder.Property(e => e.State).IsRequired();
            builder.Property(e => e.LastActivity).IsRequired();

            builder.HasOne(e => e.User)
                .WithMany(u => u.Appeals)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Messages)
                .WithOne(m => m.Appeal)
                .HasForeignKey(m => m.AppealId);
        }
    }
}