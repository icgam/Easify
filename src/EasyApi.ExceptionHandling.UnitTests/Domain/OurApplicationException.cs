using System;

namespace EasyApi.ExceptionHandling.UnitTests.Domain
{
    public sealed class OurApplicationException : ApplicationExceptionBase
    {
        public OurApplicationException() : base("message")
        {
        }

        public OurApplicationException(string message) : base(message)
        {
        }

        public OurApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}