using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.NotificationService.Persistence.Database;
using RustRetail.SharedPersistence.Database;

namespace RustRetail.NotificationService.Persistence.Repositories
{
    internal class NotificationUnitOfWork : UnitOfWork<NotificationDbContext>, INotificationUnitOfWork
    {
        public NotificationUnitOfWork(NotificationDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
        }
    }
}
