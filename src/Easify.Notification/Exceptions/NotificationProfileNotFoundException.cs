using System;

namespace Easify.Notification.Exceptions
{
    public class NotificationProfileNotFoundException : Exception
    {
        public NotificationProfileNotFoundException(string profileName) : base(
            $"Template {profileName} is not found in list of the profiles")
        {
        }
    }
}