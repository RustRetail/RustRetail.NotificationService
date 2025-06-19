using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.NotificationService.Persistence.Database;
using RustRetail.SharedPersistence.Database;

namespace RustRetail.NotificationService.Persistence.Repositories
{
    internal class UserNotificationSettingRepository : Repository<UserNotificationSetting, Guid>, IUserNotificationSettingRepository
    {
        public UserNotificationSettingRepository(NotificationDbContext context) : base(context)
        {
        }
    }
}
