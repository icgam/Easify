using System;

namespace EasyApi.ExceptionHandling.ConfigurationBuilder.Fluent
{
    public interface IHandleApplicationException
    {
        IHandleAdditionalExceptions Handle<TApplicationBaseException>()
            where TApplicationBaseException : Exception;
    }
}