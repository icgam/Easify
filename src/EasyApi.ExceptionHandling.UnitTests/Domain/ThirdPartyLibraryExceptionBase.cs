using System;

namespace EasyApi.ExceptionHandling.UnitTests.Domain
{
    public abstract class ThirdPartyLibraryExceptionBase : Exception
    {
        protected ThirdPartyLibraryExceptionBase(string message) : base(message)
        {
        }
    }
}