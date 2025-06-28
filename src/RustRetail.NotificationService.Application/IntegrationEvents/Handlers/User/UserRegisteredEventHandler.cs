using MediatR;
using RustRetail.NotificationService.Application.Abstractions.Event;
using RustRetail.NotificationService.Domain.Constants;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.SharedContracts.IntegrationEvents.V1.IdentityService.User;

namespace RustRetail.NotificationService.Application.IntegrationEvents.Handlers.User
{
    internal class UserRegisteredEventHandler(
        INotificationUnitOfWork unitOfWork)
        : INotificationHandler<IntegrationEventNotification<UserRegisteredEvent>>
    {
        readonly IUserContactInfoRepository contactInfoRepository = unitOfWork.GetRepository<IUserContactInfoRepository>();
        readonly INotificationRepository notificationRepository = unitOfWork.GetRepository<INotificationRepository>();
        readonly INotificationTemplateRepository templateRepository = unitOfWork.GetRepository<INotificationTemplateRepository>();

        public async Task Handle(
            IntegrationEventNotification<UserRegisteredEvent> notification,
            CancellationToken cancellationToken)
        {
            // Create new user contact info
            var userContact = new UserContactInfo()
            {
                Id = notification.Event.UserId,
                UserName = notification.Event.UserName,
                Email = notification.Event.Email,
            };
            await contactInfoRepository.AddAsync(userContact, cancellationToken);

            // Create notification to send welcome email to user
            var notificationMessage = new Notification()
            {
                UserId = notification.Event.UserId.ToString(),
                Title = "Welcome New User",
                Message = "Welcome new user to RustRetail e-commerce platform!",
                ActionLink = null,
                Category = NotificationCategory.Account,
                Subtype = NotificationSubtype.Account_AccountCreated,
                EntityId = notification.Event.UserId,
                EntityType = "User",
                Recipients = new List<NotificationRecipient>()
                {
                    new NotificationRecipient()
                    {
                        UserId = notification.Event.UserId,
                        Channel = NotificationChannel.Email,
                        Status = NotificationStatus.Pending,
                    }
                }
            };
            var template = await templateRepository.GetByNameAsync(NotificationTemplateName.WelcomeEmail, false, cancellationToken);
            if (template is not null)
            {
                notificationMessage.TemplateId = template.Id;
            }
            await notificationRepository.AddAsync(notificationMessage, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
