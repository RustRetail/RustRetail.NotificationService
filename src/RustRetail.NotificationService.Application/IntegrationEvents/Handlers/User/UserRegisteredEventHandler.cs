using MediatR;
using RustRetail.NotificationService.Application.Abstractions.Event;
using RustRetail.NotificationService.Application.Abstractions.Services.Core;
using RustRetail.NotificationService.Domain.Constants;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.SharedContracts.IntegrationEvents.V1.IdentityService.User;

namespace RustRetail.NotificationService.Application.IntegrationEvents.Handlers.User
{
    internal class UserRegisteredEventHandler(
        IUserContactInfoService contactInfoService,
        INotificationSchedulingService notificationSchedulingService)
        : INotificationHandler<IntegrationEventNotification<UserRegisteredEvent>>
    {
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

            // Handle notification scheduling
            var notificationSchedulingOption = NotificationSchedulingOption
                .CreateEmailNotificationOption(
                    NotificationSender.NotificationSystem,
                    "Welcome New User",
                    "Welcome new user to RustRetail e-commerce platform!",
                    NotificationCategory.Account,
                    NotificationSubtype.Account_AccountCreated,
                    [notification.Event.UserId],
                    NotificationTemplateName.WelcomeEmail)
                .SetRelatedEntity(notification.Event.UserId, "User");
            await notificationSchedulingService.ScheduleSingleRecipientEmailNotificationAsync(notificationSchedulingOption, cancellationToken: cancellationToken);
        }
    }
}
