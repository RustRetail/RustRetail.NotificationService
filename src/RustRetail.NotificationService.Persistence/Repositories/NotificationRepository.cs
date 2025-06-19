using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.NotificationService.Persistence.Database;
using RustRetail.SharedPersistence.Database;

namespace RustRetail.NotificationService.Persistence.Repositories
{
    internal class NotificationRepository : Repository<Notification, Guid>, INotificationRepository
    {
        public NotificationRepository(NotificationDbContext context) : base(context)
        {
        }
    }
}
