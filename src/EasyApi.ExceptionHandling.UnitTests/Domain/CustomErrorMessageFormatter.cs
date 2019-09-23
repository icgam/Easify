using System;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.Formatter;

namespace EasyApi.ExceptionHandling.UnitTests.Domain
{
    internal sealed class CustomErrorMessageFormatter : IErrorMessageFormatter
    {
        public string FormatErrorMessages(Error error)
        {
            throw new NotImplementedException();
        }
    }
}
