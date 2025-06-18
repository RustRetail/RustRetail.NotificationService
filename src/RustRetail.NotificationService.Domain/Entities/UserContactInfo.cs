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
    }
}
