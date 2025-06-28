using RustRetail.NotificationService.Domain.Entities;

namespace RustRetail.NotificationService.Application.Abstractions.Services.Core
{
    public interface IUserContactInfoService
    {
        Task AddOrUpdateUserContactInfoAsync(UserContactInfo contactInfo, CancellationToken cancellationToken = default);
    }
}
