using System;
using System.Collections.Generic;
using System.Linq;
using Easify.Notification.Messaging;

// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace Easify.Notification
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