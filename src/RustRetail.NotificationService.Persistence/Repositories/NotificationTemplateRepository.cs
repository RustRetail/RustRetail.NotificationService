using Microsoft.EntityFrameworkCore;
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

        public async Task<NotificationTemplate?> GetByNameAsync(
            string name,
            bool asTracking = true,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();
            query = asTracking ? query.AsTracking() : query.AsNoTracking();

            return await query.FirstOrDefaultAsync(
                t => t.Name == name,
                cancellationToken);
        }
    }
}
