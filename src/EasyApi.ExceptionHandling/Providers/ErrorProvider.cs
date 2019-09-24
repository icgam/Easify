// This software is part of the EasyApi framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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