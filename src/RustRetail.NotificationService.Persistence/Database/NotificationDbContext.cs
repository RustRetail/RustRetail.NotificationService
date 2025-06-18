using Microsoft.EntityFrameworkCore;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.SharedKernel.Domain.Models;
using RustRetail.SharedPersistence.Abstraction;

namespace RustRetail.NotificationService.Persistence.Database
{
    public class NotificationDbContext : DbContext, IHasOutboxMessage
    {
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<UserContactInfo> UserContactInfos { get; set; }
        public DbSet<UserNotificationSetting> UserNotificationSettings { get; set; }
        public DbSet<OutboxMessage> OutboxMessage { get; set; }

        public NotificationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OutboxMessage>().ToTable("OutboxMessages");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
        }
    }
}
