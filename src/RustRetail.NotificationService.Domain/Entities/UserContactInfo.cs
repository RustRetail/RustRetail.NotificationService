using RustRetail.SharedKernel.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RustRetail.NotificationService.Domain.Entities
{
    public sealed class UserContactInfo : AggregateRoot<Guid>
    {
        // Id of UserContactInfo is same as UserId in the Identity service

        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? PushNotificationToken { get; set; }
        public string? PreferredLanguage { get; set; }
    }
}
