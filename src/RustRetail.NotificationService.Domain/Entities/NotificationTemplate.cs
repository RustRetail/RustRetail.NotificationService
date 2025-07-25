﻿using RustRetail.NotificationService.Domain.Constants;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.SharedKernel.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace RustRetail.NotificationService.Domain.Entities
{
    public sealed class NotificationTemplate : AggregateRoot<Guid>
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Subject { get; set; } = string.Empty;
        [Required]
        public string Body { get; set; } = string.Empty;
        public string? Footer { get; set; }
        public string? Header { get; set; }
        public string? DefaultActionLink { get; set; }
        [Required]
        public NotificationCategory Category { get; set; } = NotificationCategory.Account;
        [Required]
        public string Subtype { get; set; } = NotificationSubtype.Account_AccountCreated;
        [Required]
        public NotificationChannel Channel { get; set; } = NotificationChannel.Email;
        public bool IsActive { get; set; } = true;

        public ICollection<Notification> Notifications { get; set; } = [];

        public string RenderBodyWithUserContact(UserContactInfo userContactInfo)
        {
            return Body
                .Replace("{UserName}", userContactInfo.UserName)
                .Replace("{UserEmail}", userContactInfo.Email)
                .Replace("{UserPhone}", userContactInfo.PhoneNumber ?? string.Empty);
        }

        public string RenderBody(UserContactInfo userContactInfo, Dictionary<string, object>? valuePairs = null)
        {
            StringBuilder builder = new StringBuilder(Body);

            var replacements = new Dictionary<string, string>
            {
                { "UserName", userContactInfo.UserName },
                { "UserEmail", userContactInfo.Email },
                { "UserPhone", userContactInfo.PhoneNumber ?? string.Empty }
            };

            if (valuePairs is not null)
            {
                foreach (var pair in valuePairs)
                {
                    replacements[pair.Key] = Convert.ToString(pair.Value, CultureInfo.InvariantCulture) ?? string.Empty;
                }
            }

            foreach (var replacement in replacements)
            {
                builder.Replace($"{{{replacement.Key}}}", replacement.Value);
            }

            return builder.ToString();
        }
    }
}
