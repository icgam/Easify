using System;

namespace EasyApi.Sample.WebAPI.Domain
{
    public abstract class TemplateApiApplicationException : Exception
    {
        protected TemplateApiApplicationException(string message) : base(message)
        {
        }

        protected TemplateApiApplicationException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}