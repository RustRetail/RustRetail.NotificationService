using RustRetail.SharedKernel.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RustRetail.NotificationService.Domain.Entities
{
    public sealed class UserContactInfo : AggregateRoot<Guid>
    {
        // Id of UserContactInfo is same as UserId in the Identity service

        [Required]
        [MaxLength(256)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        public string? PushNotificationToken { get; set; }
        [MaxLength(100)]
        public string? PreferredLanguage { get; set; }

        public UserContactInfo()
        {
        }

        public static UserContactInfo Create(Guid userId,
            string username,
            string email,
            string? phoneNumber = null,
            string? pushNotificationToken = null,
            string? preferredLanguage = null)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }
            return new UserContactInfo()
            {
                Id = userId,
                UserName = username,
                Email = email,
                PhoneNumber = phoneNumber,
                PushNotificationToken = pushNotificationToken,
                PreferredLanguage = preferredLanguage,
                CreatedDateTime = DateTimeOffset.UtcNow
            };
        }
    }
}
