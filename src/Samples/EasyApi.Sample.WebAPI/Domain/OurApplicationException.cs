using System;

namespace EasyApi.Sample.WebAPI.Domain
{
    public sealed class OurApplicationException : TemplateApiApplicationException
    {
        public OurApplicationException() : base("I am a user friendly exception message")
        {
        }

        public OurApplicationException(string message) : base(message)
        {
        }
        public OurApplicationException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}