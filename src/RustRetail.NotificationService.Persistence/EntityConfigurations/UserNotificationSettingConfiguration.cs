using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Enums;

namespace RustRetail.NotificationService.Persistence.EntityConfigurations
{
    internal class UserNotificationSettingConfiguration : IEntityTypeConfiguration<UserNotificationSetting>
    {
        const string TableName = "UserNotificationSettings";
        public void Configure(EntityTypeBuilder<UserNotificationSetting> builder)
        {
            builder.ToTable(TableName);

            builder.Property(x => x.Category)
                .HasConversion(
                    category => category.Value,
                    value => NotificationCategory.FromValue<NotificationCategory>(value)
                );
            builder.Property(x => x.Channel)
                .HasConversion(
                    channel => channel.Value,
                    value => NotificationChannel.FromValue<NotificationChannel>(value)
                );

            builder.HasIndex(x => new { x.UserId, x.Category, x.Channel })
                .IsUnique();
        }
    }
}
