using System;
using System.Collections.Generic;

namespace Easify.Notification.Messaging
{
    public sealed class Message
    {
        private Message(string subject, string content, EmailAddress sender,
            IEnumerable<EmailAddress> recipients)
        {
            Subject = subject;
            Content = content;
            Sender = sender ?? throw new ArgumentNullException(nameof(sender));
            Recipients = recipients ?? throw new ArgumentNullException(nameof(recipients));
        }

        public string Subject { get; }
        public string Content { get; }
        public EmailAddress Sender { get; }
        public IEnumerable<EmailAddress> Recipients { get; }

        public static Message From(string title, string contents, EmailAddress sender,
            IEnumerable<EmailAddress> recipients)
        {
            return new Message(title, contents, sender, recipients);
        }
    }
}