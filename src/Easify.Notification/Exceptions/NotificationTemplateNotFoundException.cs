using System;

namespace Easify.Notification.Exceptions
{
    public class NotificationTemplateNotFoundException : Exception
    {
        public NotificationTemplateNotFoundException(string templateName) : base(
            $"Template {templateName} is not found in configuration")
        {
        }
    }
}