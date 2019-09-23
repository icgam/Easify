using System;

namespace EasyApi.Logging
{
    public sealed class LogMessage
    {
        public LogMessage(DateTime loggedAt, string level, string message)
        {
            LoggedAt = loggedAt;
            Level = level;
            Message = message;
        }

        public DateTime LoggedAt { get; }
        public string Level { get; }
        public string Message { get; }
    }
}
