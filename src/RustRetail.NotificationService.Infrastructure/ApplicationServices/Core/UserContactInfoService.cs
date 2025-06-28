using RustRetail.NotificationService.Application.Abstractions.Services.Core;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Repositories;

namespace RustRetail.NotificationService.Infrastructure.ApplicationServices.Core
{
    internal class UserContactInfoService : IUserContactInfoService
    {
        readonly INotificationUnitOfWork _unitOfWork;
        readonly IUserContactInfoRepository _contactInfoRepository;

        public UserContactInfoService(INotificationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _contactInfoRepository = unitOfWork.GetRepository<IUserContactInfoRepository>();
        }

        public async Task AddOrUpdateUserContactInfoAsync(
            UserContactInfo contactInfo,
            CancellationToken cancellationToken = default)
        {
            var existingContactInfo = await _contactInfoRepository.GetByIdAsync(
                contactInfo.Id,
                asNoTracking: false,
                cancellationToken);
            // Add new
            if (existingContactInfo is null)
            {
                await _contactInfoRepository.AddAsync(contactInfo, cancellationToken);
            }
            // Update
            else
            {
                existingContactInfo.UserName = contactInfo.UserName;
                existingContactInfo.Email = contactInfo.Email;
                existingContactInfo.PhoneNumber = contactInfo.PhoneNumber;
                existingContactInfo.PushNotificationToken = contactInfo.PushNotificationToken;
                existingContactInfo.PreferredLanguage = contactInfo.PreferredLanguage;
                existingContactInfo.SetUpdatedDateTime(DateTimeOffset.UtcNow);
                _contactInfoRepository.Update(existingContactInfo);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
