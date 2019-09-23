using System;
using System.Collections.Generic;
using EasyApi.ExceptionHandling.Domain;

namespace EasyApi.ExceptionHandling.ErrorBuilder
{
    public sealed class DefaultErrorBuilder<TException> : IErrorBuilder<TException> where TException : Exception
    {
        public Error Build(TException exception, IEnumerable<Error> internalErrors, bool includeSystemLevelExceptions)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            if (internalErrors == null) throw new ArgumentNullException(nameof(internalErrors));
            return new Error(exception.Message, exception.GetType().Name, internalErrors);
        }
    }
}