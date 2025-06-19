using RustRetail.NotificationService.Domain.Entities;
using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.NotificationService.Domain.Repositories
{
    public interface INotificationTemplateRepository : IRepository<NotificationTemplate, Guid>
    {
    }
}
