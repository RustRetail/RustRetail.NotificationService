using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Enums;

namespace RustRetail.NotificationService.Persistence.EntityConfigurations
{
    internal class NotificationRecipientConfiguration : IEntityTypeConfiguration<NotificationRecipient>
    {
        const string TableName = "NotificationRecipients";

        public void Configure(EntityTypeBuilder<NotificationRecipient> builder)
        {
            builder.ToTable(TableName);

            builder.Property(x => x.Channel)
                .HasConversion(
                    channel => channel.Value,
                    value => NotificationChannel.FromValue<NotificationChannel>(value)
                );
            builder.Property(x => x.Status)
                .HasConversion(
                    status => status.Value,
                    value => NotificationStatus.FromValue<NotificationStatus>(value)
                );

            builder.HasIndex(x => new { x.NotificationId, x.UserId })
                .IsUnique();
        }
    }
}
