using Domain.Appeals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class AppealMessageConfiguration : IEntityTypeConfiguration<AppealMessage>
    {
        public void Configure(EntityTypeBuilder<AppealMessage> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.AppealId).IsRequired();
            builder.Property(e => e.Message).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();

            builder.HasOne(e => e.Appeal)
                  .WithMany(a => a.Messages)
                  .HasForeignKey(e => e.AppealId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Files)
                  .WithOne(f => f.Message)
                  .HasForeignKey(f => f.MessageId);
        }
    }
}