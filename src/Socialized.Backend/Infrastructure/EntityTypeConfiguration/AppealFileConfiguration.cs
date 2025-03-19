using Domain.Appeals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class AppealFileConfiguration : IEntityTypeConfiguration<AppealFile>
    {
        public void Configure(EntityTypeBuilder<AppealFile> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.MessageId).IsRequired();
            builder.Property(e => e.RelativePath).IsRequired();
            builder.HasOne(e => e.Message)
                  .WithMany(m => m.Files)
                  .HasForeignKey(e => e.MessageId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}