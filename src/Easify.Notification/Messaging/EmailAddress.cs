namespace Easify.Notification.Messaging
{
    public sealed class EmailAddress
    {
        private EmailAddress(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; }
        public string Name { get; }

        public static EmailAddress From(string email)
        {
            return new EmailAddress(email, email);
        }

        public static EmailAddress From(string email, string name)
        {
            return new EmailAddress(email, name);
        }
    }
}