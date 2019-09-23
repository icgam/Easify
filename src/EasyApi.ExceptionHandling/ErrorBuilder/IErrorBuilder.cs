using System;
using System.Collections.Generic;
using EasyApi.ExceptionHandling.Domain;

namespace EasyApi.ExceptionHandling.ErrorBuilder
{
    public interface IErrorBuilder<TException> where TException : Exception
    {
        Error Build(TException exception, IEnumerable<Error> internalErrors, bool includeSystemLevelExceptions);
    }
}