using System;

namespace EasyApi.ExceptionHandling.UnitTests.Domain
{
    public sealed class ExceptionWithCustomContent : Exception
    {
        public ExceptionWithCustomContent(string content)
        {
            Content = content;
        }

        public string Content { get; }
    }
}
