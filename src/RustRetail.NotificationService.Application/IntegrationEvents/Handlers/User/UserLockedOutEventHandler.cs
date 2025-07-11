using MediatR;
using RustRetail.NotificationService.Application.Abstractions.Event;
using RustRetail.NotificationService.Application.Abstractions.Services.Core;
using RustRetail.NotificationService.Domain.Constants;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.SharedContracts.IntegrationEvents.V1.IdentityService.Authentication;

namespace RustRetail.NotificationService.Application.IntegrationEvents.Handlers.User
{
    internal class UserLockedOutEventHandler(
        INotificationSchedulingService notificationSchedulingService)
        : INotificationHandler<IntegrationEventNotification<UserLockedOutEvent>>
    {
        public async Task Handle(
            IntegrationEventNotification<UserLockedOutEvent> notification,
            CancellationToken cancellationToken)
        {
            var notificationSchedulingOption = NotificationSchedulingOption
                .CreateEmailNotificationOption(
                    NotificationSender.NotificationSystem,
                    "User Locked Out",
                    "User locked out from RustRetail e-commerce platform!",
                    NotificationCategory.Account,
                    NotificationSubtype.Account_LockedOut,
                    [notification.Event.UserId],
                    NotificationTemplateName.UserLockedOutEmail)
                .SetRelatedEntity(notification.Event.UserId, "User");
            await notificationSchedulingService.ScheduleSingleRecipientEmailNotificationAsync(notificationSchedulingOption,
                new Dictionary<string, object>(3)
                {
                    {"Reason", notification.Event.Reason},
                    {"LockoutStart", new DateTimeOffset(notification.Event.OccurredOn)},
                    {"LockoutEnd", DateTimeOffset.UtcNow.AddMilliseconds(notification.Event.LockoutDurationInMilliseconds) }
                }, cancellationToken);
        }
    }
}
