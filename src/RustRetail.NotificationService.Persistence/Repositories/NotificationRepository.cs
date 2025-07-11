using Microsoft.EntityFrameworkCore;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.NotificationService.Persistence.Database;
using RustRetail.SharedPersistence.Database;
using System.Linq.Expressions;

namespace RustRetail.NotificationService.Persistence.Repositories
{
    internal class NotificationRepository : Repository<Notification, Guid>, INotificationRepository
    {
        public NotificationRepository(NotificationDbContext context) : base(context)
        {
        }

        public async Task<Notification?> GetNotificationByIdAsync(Guid notificationId, bool asTracking = true, bool asSplitQuery = false, CancellationToken cancellationToken = default, params Expression<Func<Notification, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            query = asTracking ? query.AsTracking() : query.AsNoTracking();

            query = asSplitQuery ? query.AsSplitQuery() : query;

            return await query.FirstOrDefaultAsync(
                n => n.Id == notificationId,
                cancellationToken);
        }
    }
}
