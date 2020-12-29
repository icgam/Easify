using System;

namespace Easify.Notification.Exceptions
{
    public class SmtpOptionsException : Exception
    {
        public SmtpOptionsException(string propertyName) : base($"{propertyName} is missing from the configuration")
        {
        }
    }
}