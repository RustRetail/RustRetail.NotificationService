using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Enums;

namespace RustRetail.NotificationService.Persistence.EntityConfigurations
{
    internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        const string TableName = "Notifications";

        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(TableName);

            builder.Property(x => x.Category)
                .HasConversion(
                    category => category.Value,
                    value => NotificationCategory.FromValue<NotificationCategory>(value)
                );

            builder.HasMany(x => x.Recipients)
                .WithOne(x => x.Notification)
                .HasForeignKey(x => x.NotificationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
