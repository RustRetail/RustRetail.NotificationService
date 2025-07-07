using Microsoft.EntityFrameworkCore;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.NotificationService.Persistence.Database;
using RustRetail.SharedPersistence.Database;

namespace RustRetail.NotificationService.Persistence.Repositories
{
    internal class NotificationRepository : Repository<Notification, Guid>, INotificationRepository
    {
        const int MaxTakeSize = 100;
        const int MaxRecipientsCount = 50;

        public NotificationRepository(NotificationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetPendingEmailNotificationsAsync(
            int takeSize,
            bool asTracking = true,
            CancellationToken cancellationToken = default)
        {
            QueryTrackingBehavior trackingBehavior = asTracking
                ? QueryTrackingBehavior.TrackAll
                : QueryTrackingBehavior.NoTracking;

            return await _dbSet
                .AsTracking(trackingBehavior)
                .Include(n => n.Recipients)
                .Where(n => n.Recipients.Any(r => r.Status == NotificationStatus.Pending)
                    && n.Recipients.Any(r => r.Channel == NotificationChannel.Email)
                    && n.Recipients.Count() <= MaxRecipientsCount)
                .OrderByDescending(n => n.CreatedDateTime)
                .Take(takeSize <= MaxTakeSize ? takeSize : MaxTakeSize)
                .AsSplitQuery()
                .ToListAsync(cancellationToken);
        }
    }
}
