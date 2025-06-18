using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Enums;

namespace RustRetail.NotificationService.Persistence.EntityConfigurations
{
    internal class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
    {
        const string TableName = "NotificationTemplates";
        public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
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

            builder.HasMany(x => x.Notifications)
                .WithOne(x => x.Template)
                .HasForeignKey(x => x.TemplateId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
