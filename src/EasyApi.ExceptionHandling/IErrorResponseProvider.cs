using System;
using EasyApi.ExceptionHandling.Domain;

namespace EasyApi.ExceptionHandling
{
    public interface IErrorResponseProvider
    {
        InternalErrorResponse GetErrors<TException>(TException exception)
            where TException : Exception;
    }
}