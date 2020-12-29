using System;
using System.Linq;
using System.Threading.Tasks;
using Easify.Notification.Configuration;
using Easify.Notification.Exceptions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Easify.Notification.Messaging
{
    public sealed class MailKitMessagingService : IMessagingService
    {
        private readonly ILogger<MailKitMessagingService> _logger;
        private readonly SmtpOptions _smtpOptions;

        public MailKitMessagingService(IOptions<SmtpOptions> smtpOptionsAccessor, ILogger<MailKitMessagingService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _smtpOptions = smtpOptionsAccessor?.Value ?? throw new ArgumentNullException(nameof(smtpOptionsAccessor));
        }

        public async Task SendAsync(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            EnsureOptions();

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(CreateAddress(message.Sender));
            emailMessage.To.AddRange(message.Recipients.Select(CreateAddress).ToList());
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart("html") {Text = message.Content};

            await SendEmailMessageAsync(emailMessage);
        }

        private void EnsureOptions()
        {
            if (string.IsNullOrWhiteSpace(_smtpOptions.LocalDomain))
                throw new SmtpOptionsException(nameof(_smtpOptions.LocalDomain));

            if (string.IsNullOrWhiteSpace(_smtpOptions.Server))
                throw new SmtpOptionsException(nameof(_smtpOptions.Server));
        }

        private async Task SendEmailMessageAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.LocalDomain = _smtpOptions.LocalDomain;
                await client.ConnectAsync(_smtpOptions.Server, _smtpOptions.Port, SecureSocketOptions.None);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        private MailboxAddress CreateAddress(EmailAddress recipient)
        {
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));

            return new MailboxAddress(recipient.Name ?? recipient.Email, recipient.Email);
        }
    }
}