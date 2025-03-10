using Domain.Appeals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class AppealMessageReplyConfiguration : IEntityTypeConfiguration<AppealMessageReply>
    {
        public void Configure(EntityTypeBuilder<AppealMessageReply> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.AppealMessageId).IsRequired();
            builder.Property(e => e.Reply).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();

            builder.HasOne(e => e.Message)
                  .WithMany(m => m.AppealMessageReplies)
                  .HasForeignKey(e => e.AppealMessageId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}