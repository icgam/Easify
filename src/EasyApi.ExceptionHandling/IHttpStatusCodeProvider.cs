using System;
using System.Net;

namespace EasyApi.ExceptionHandling
{
    public interface IHttpStatusCodeProvider
    {
        HttpStatusCode GetHttpStatusCode<TException>(TException exception)
            where TException : Exception;
    }
}