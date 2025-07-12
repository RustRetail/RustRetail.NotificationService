using MediatR;
using RustRetail.NotificationService.Application.Abstractions.Event;
using RustRetail.NotificationService.Application.Abstractions.Services.Core;
using RustRetail.NotificationService.Domain.Constants;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.SharedContracts.IntegrationEvents.V1.IdentityService.User;

namespace RustRetail.NotificationService.Application.IntegrationEvents.Handlers.User
{
    internal class UserUpdateProfileEventHandler(
        INotificationSchedulingService notificationSchedulingService)
        : INotificationHandler<IntegrationEventNotification<UserUpdatedProfileEvent>>
    {
        public async Task Handle(IntegrationEventNotification<UserUpdatedProfileEvent> notification, CancellationToken cancellationToken)
        {
            var notificationSchedulingOption = new NotificationSchedulingOption()
            {
                SenderId = NotificationSender.NotificationSystem,
                Title = "User Profile Updated",
                Message = "User profile has been updated successfully.",
                Category = NotificationCategory.Account,
                Subtype = NotificationSubtype.Account_AccountUpdated,
                Channel = NotificationChannel.InApp,
                RecipientIds = [notification.Event.UserId]
            }.SetRelatedEntity(notification.Event.UserId, "User");
            await notificationSchedulingService.ScheduleNotificationAsync(notificationSchedulingOption, cancellationToken);
        }
    }
}
