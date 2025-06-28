using MediatR;
using RustRetail.NotificationService.Application.Abstractions.Event;
using RustRetail.NotificationService.Application.Abstractions.Services.Core;
using RustRetail.NotificationService.Domain.Constants;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.SharedContracts.IntegrationEvents.V1.IdentityService.User;

namespace RustRetail.NotificationService.Application.IntegrationEvents.Handlers.User
{
    internal class UserRegisteredEventHandler(
        INotificationUnitOfWork unitOfWork,
        IUserContactInfoService contactInfoService)
        : INotificationHandler<IntegrationEventNotification<UserRegisteredEvent>>
    {
        readonly INotificationRepository notificationRepository = unitOfWork.GetRepository<INotificationRepository>();
        readonly INotificationTemplateRepository templateRepository = unitOfWork.GetRepository<INotificationTemplateRepository>();

        public async Task Handle(
            IntegrationEventNotification<UserRegisteredEvent> notification,
            CancellationToken cancellationToken)
        {
            // Handle user's contact info
            var userContact = UserContactInfo.Create(
                notification.Event.UserId,
                notification.Event.UserName,
                notification.Event.Email);
            await contactInfoService.AddOrUpdateUserContactInfoAsync(userContact, cancellationToken);

            // Create notification to send welcome email to user
            var newNotification = Notification.Create(NotificationSender.NotificationSystem,
                "Welcome New User",
                "Welcome new user to RustRetail e-commerce platform!",
                NotificationCategory.Account,
                NotificationSubtype.Account_AccountCreated,
                entityId: notification.Event.UserId,
                entityType: "User");
            newNotification.AddRecipients([NotificationRecipient.Create(
                notification.Event.UserId,
                NotificationChannel.Email)]);
            var template = await templateRepository.GetByNameAsync(NotificationTemplateName.WelcomeEmail, false, cancellationToken);
            if (template is not null)
            {
                newNotification.SetTemplate(template);
            }
            await notificationRepository.AddAsync(newNotification, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
