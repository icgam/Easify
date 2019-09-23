using System;

namespace EasyApi.ExceptionHandling.UnitTests.Domain
{
    public class OurDerivedApplicationException : AnotherOurApplicationException
    {
        public OurDerivedApplicationException(string message) : base(message)
        {
        }

        public OurDerivedApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}