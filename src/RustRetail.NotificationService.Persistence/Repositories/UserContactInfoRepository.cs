using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.NotificationService.Persistence.Database;
using RustRetail.SharedPersistence.Database;

namespace RustRetail.NotificationService.Persistence.Repositories
{
    internal class UserContactInfoRepository : Repository<UserContactInfo, Guid>, IUserContactInfoRepository
    {
        public UserContactInfoRepository(NotificationDbContext context) : base(context)
        {
        }
    }
}
