namespace Easify.Notification.Configuration
{
    public sealed class NotificationProfile
    {
        public string ProfileName { get; set; }
        public NotificationAudience[] Audiences { get; set; }
    }
}