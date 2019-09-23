using System;
using System.Collections.Generic;
using System.Linq;
using EasyApi.ExceptionHandling.Domain;

namespace EasyApi.ExceptionHandling.Providers
{
    public sealed class ErrorProvider : IErrorProvider
    {
        public Error ExtractErrorsFor<TException>(TException exception, IErrorProviderOptions options)
            where TException : Exception
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            if (options == null) throw new ArgumentNullException(nameof(options));

            var internalErrors = new List<Error>();

            if (ContainsNestedException(exception))
                internalErrors.AddRange(ProcessInternalException(exception.InnerException, options));

            if (ShouldExtractMessage(exception, options))
                return BuildError(exception, internalErrors, options);

            if (internalErrors.Count == 1)
                return internalErrors.Single();

            return options.GenericError;
        }

        private bool ShouldExtractMessage<TException>(TException exception, IErrorProviderOptions options)
            where TException : Exception
        {
            return options.IncludeSystemLevelExceptions || ApplicationException(exception, options);
        }

        private bool ApplicationException<TException>(TException exception, IErrorProviderOptions options)
            where TException : Exception
        {
            return options.ExceptionsToHandle.Any(et => et.CanHandle(exception));
        }

        private bool ContainsNestedException(Exception exception)
        {
            return exception.InnerException != null;
        }

        private List<Error> ProcessInternalException<TException>(TException exception, IErrorProviderOptions options)
            where TException : Exception
        {
            var errors = new List<Error>();

            if (exception is AggregateException)
            {
                var aggregate = exception as AggregateException;
                foreach (var innerException in aggregate.InnerExceptions)
                    errors.AddRange(ProcessInternalException(innerException, options));
            }
            else
            {
                if (ContainsNestedException(exception))
                    errors = ProcessInternalException(exception.InnerException, options);

                if (ShouldExtractMessage(exception, options))
                    errors = new List<Error>
                    {
                        BuildError(exception, errors, options)
                    };
            }

            return errors;
        }

        private Error BuildError<TException>(TException exception, IEnumerable<Error> internalErrors,
            IErrorProviderOptions options)
            where TException : Exception
        {
            var rule = options.ExceptionsToHandle.FirstOrDefault(r => r.CanHandle(exception));
            return rule == null
                ? new Error(exception.Message, exception.GetType().Name, internalErrors)
                : rule.GetError(exception, internalErrors, options.IncludeSystemLevelExceptions);
        }
    }
}