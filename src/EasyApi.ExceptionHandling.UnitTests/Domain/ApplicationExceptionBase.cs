using System;

namespace EasyApi.ExceptionHandling.UnitTests.Domain
{
    public abstract class ApplicationExceptionBase : Exception
    {
        protected ApplicationExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApplicationExceptionBase(string message) : base(message)
        {
        }
    }
}