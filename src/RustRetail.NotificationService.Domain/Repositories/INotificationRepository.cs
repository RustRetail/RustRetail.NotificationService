using RustRetail.NotificationService.Domain.Entities;
using RustRetail.SharedKernel.Domain.Abstractions;
using System.Linq.Expressions;

namespace RustRetail.NotificationService.Domain.Repositories
{
    public interface INotificationRepository : IRepository<Notification, Guid>
    {
        Task<IEnumerable<Notification>> GetPendingEmailNotificationsAsync(int takeSize, bool asTracking = true, CancellationToken cancellationToken = default);
        Task<Notification?> GetNotificationByIdAsync(Guid notificationId,
            bool asTracking = true,
            bool asSplitQuery = false,
            CancellationToken cancellationToken = default,
            params Expression<Func<Notification, object>>[] includes);
    }
}
