using System;
using EasyApi.ExceptionHandling.Domain;

namespace EasyApi.ExceptionHandling
{
    public interface IErrorProvider
    {
        Error ExtractErrorsFor<TException>(TException exception, IErrorProviderOptions options)
            where TException : Exception;
    }
}