using System;

namespace EasyApi.ExceptionHandling.UnitTests.Domain
{
    public class AnotherOurApplicationException : ApplicationExceptionBase
    {
        public AnotherOurApplicationException(string message) : base(message)
        {
        }

        public AnotherOurApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}