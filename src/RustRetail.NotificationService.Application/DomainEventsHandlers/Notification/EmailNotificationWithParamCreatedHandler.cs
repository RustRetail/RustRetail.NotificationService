using RustRetail.NotificationService.Domain.Events.Notification.Email;
using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.SharedApplication.Abstractions;
using RustRetail.SharedApplication.Behaviors.Messaging;
using RustRetail.SharedContracts.IntegrationEvents.V1.NotificationService.Email;

namespace RustRetail.NotificationService.Application.DomainEventsHandlers.Notification
{
    internal class EmailNotificationWithParamCreatedHandler(
        IMessageBus messageBus,
        INotificationUnitOfWork unitOfWork)
        : IDomainEventHandler<DomainEventNotification<EmailNotificationWithParamCreated>>
    {
        readonly INotificationRepository _notificationRepository = unitOfWork.GetRepository<INotificationRepository>();
        readonly IUserContactInfoRepository _userContactInfoRepository = unitOfWork.GetRepository<IUserContactInfoRepository>();
        readonly INotificationTemplateRepository _templateRepository = unitOfWork.GetRepository<INotificationTemplateRepository>();

        public async Task Handle(DomainEventNotification<EmailNotificationWithParamCreated> notification, CancellationToken cancellationToken)
        {
            var emailNotification = await _notificationRepository.GetNotificationByIdAsync(notification.DomainEvent.NotificationId,
                asTracking: false,
                asSplitQuery: true,
                cancellationToken: cancellationToken,
                n => n.Recipients);

            ArgumentNullException.ThrowIfNull(emailNotification, nameof(emailNotification));
            ArgumentNullException.ThrowIfNull(emailNotification.TemplateId, nameof(emailNotification.TemplateId));

            var template = await _templateRepository
                .GetByIdAsync(emailNotification.TemplateId.Value,
                    true,
                    cancellationToken);
            if (template is null || !template.IsActive)
            {
                throw new InvalidOperationException($"Email notification template with id {emailNotification.TemplateId} is not found or not active.");
            }

            var userContact = await _userContactInfoRepository
                .GetByIdAsync(emailNotification.Recipients.First().UserId,
                    true,
                    cancellationToken);
            ArgumentNullException.ThrowIfNull(userContact, nameof(userContact));

            var renderedBody = template.RenderBody(userContact, notification.DomainEvent.Parameters);
            await messageBus.PublishAsync(new EmailSendRequestedEvent(notification.DomainEvent.NotificationId, userContact.Email, template.Subject, renderedBody),
                cancellationToken);
        }
    }
}
