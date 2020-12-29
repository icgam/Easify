namespace Easify.Notification.Configuration
{
    public sealed class SmtpOptions
    {
        public string Server { get; set; }
        public int Port { get; set; } = 25;
        public string LocalDomain { get; set; }
    }
}