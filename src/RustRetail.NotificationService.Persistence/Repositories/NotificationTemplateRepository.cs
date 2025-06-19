using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.NotificationService.Persistence.Database;
using RustRetail.SharedPersistence.Database;

namespace RustRetail.NotificationService.Persistence.Repositories
{
    internal class NotificationTemplateRepository : Repository<NotificationTemplate, Guid>, INotificationTemplateRepository
    {
        public NotificationTemplateRepository(NotificationDbContext context) : base(context)
        {
        }
    }
}
