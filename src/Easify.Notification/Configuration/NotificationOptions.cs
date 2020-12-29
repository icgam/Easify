namespace Easify.Notification.Configuration
{
    public sealed class NotificationOptions
    {
        public string Sender { get; set; }
        public NotificationProfile[] Profiles { get; set; } = { };
        public NotificationTemplate[] Templates { get; set; } = { };
    }
}