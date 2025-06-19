using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RustRetail.NotificationService.Application.Abstractions.Services.Email;
using System.Net;
using System.Net.Mail;

namespace RustRetail.NotificationService.Infrastructure.ApplicationServices.Email.Gmail
{
    internal class GmailEmailService : IEmailService
    {
        readonly GmailEmailOptions _emailOptions;
        readonly SmtpClient _smtpClient;
        readonly ILogger<GmailEmailService> _logger;
        public GmailEmailService(IOptions<GmailEmailOptions> emailOptions, ILogger<GmailEmailService> logger)
        {
            _emailOptions = emailOptions?.Value ?? throw new ArgumentNullException(nameof(emailOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _smtpClient = new SmtpClient(_emailOptions.Host)
            {
                Port = _emailOptions.Port,
                Credentials = new NetworkCredential(_emailOptions.UserName, _emailOptions.Password),
                EnableSsl = true,
            };
        }

        public async Task SendEmailAsync(
            IEnumerable<string> recipients,
            string subject,
            string body,
            IEnumerable<string>? ccRecipients = null,
            IEnumerable<string>? bccRecipients = null,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(recipients);
            if (!recipients.Any())
            {
                throw new ArgumentException("At least one recipient must be provided.", nameof(recipients));
            }
            ArgumentException.ThrowIfNullOrWhiteSpace(subject);
            ArgumentException.ThrowIfNullOrWhiteSpace(body);

            var mailMessage = GetMailMessage(subject, body);
            foreach (var recipient in recipients)
            {
                mailMessage.To.Add(new MailAddress(recipient));
            }
            if (ccRecipients != null)
            {
                foreach (var cc in ccRecipients)
                {
                    mailMessage.CC.Add(new MailAddress(cc));
                }
            }
            if (bccRecipients != null)
            {
                foreach (var bcc in bccRecipients)
                {
                    mailMessage.Bcc.Add(new MailAddress(bcc));
                }
            }
            try
            {
                await _smtpClient.SendMailAsync(mailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipients}", string.Join(", ", recipients));
            }
        }

        private MailMessage GetMailMessage(string subject, string body, bool isHtmlBody = true)
            => new MailMessage
            {
                From = new MailAddress(_emailOptions.FromEmail, _emailOptions.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtmlBody,
            };
    }
}
