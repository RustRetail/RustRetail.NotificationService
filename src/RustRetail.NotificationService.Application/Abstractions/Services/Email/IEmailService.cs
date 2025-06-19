namespace RustRetail.NotificationService.Application.Abstractions.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            IEnumerable<string> recipients,
            string subject,
            string body,
            IEnumerable<string>? ccRecipients = default,
            IEnumerable<string>? bccRecipients = default,
            CancellationToken cancellationToken = default);
    }
}
