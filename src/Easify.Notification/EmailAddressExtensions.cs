using System;
using System.Collections.Generic;
using System.Linq;
using Easify.Notification.Messaging;

namespace Easify.Notification.Extensions
{
    public static class EmailAddressExtensions
    {
        public const string AddressSeparator = ";";
        public const string EmailAndNameSeparator = "|";

        public static EmailAddress ToEmailAddress(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            var cleanSource = source.Replace(AddressSeparator, string.Empty);

            if (!cleanSource.Contains(EmailAndNameSeparator))
                return EmailAddress.From(cleanSource);

            var emailAddress = cleanSource.Split(new[] {EmailAndNameSeparator}, StringSplitOptions.RemoveEmptyEntries);
            return EmailAddress.From(emailAddress[0], emailAddress[1]);
        }

        public static IEnumerable<EmailAddress> ToEmailAddresses(this string source)
        {
            var emailsToParse =
                source.Split(new[] {AddressSeparator}, StringSplitOptions.RemoveEmptyEntries).ToList();
            return emailsToParse.Select(value => value.ToEmailAddress()).Where(value => value != null).ToList();
        }
    }
}