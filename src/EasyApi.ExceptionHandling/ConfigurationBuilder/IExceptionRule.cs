using System;
using System.Collections.Generic;
using EasyApi.ExceptionHandling.Domain;

namespace EasyApi.ExceptionHandling.ConfigurationBuilder
{
    public interface IExceptionRule
    {
        string TypeFullName { get; }

        bool CanHandle<TExceptionToCheck>(TExceptionToCheck exception)
            where TExceptionToCheck : Exception;

        Error GetError<TExceptionToCheck>(TExceptionToCheck exception, IEnumerable<Error> internalErrors,
            bool includeSystemLevelExceptions)
            where TExceptionToCheck : Exception;
    }
}