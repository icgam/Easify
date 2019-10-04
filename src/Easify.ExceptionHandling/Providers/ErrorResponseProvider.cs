// This software is part of the Easify framework
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
using Easify.ExceptionHandling.Domain;
using Easify.ExceptionHandling.Formatter;

namespace Easify.ExceptionHandling.Providers
{
    public sealed class ErrorResponseProvider : IErrorResponseProvider
    {
        private readonly IErrorMessageFormatter _errorMessageFormatter;
        private readonly IErrorProvider _errorProvider;
        private readonly IErrorResponseProviderOptions _options;
        private readonly IHttpStatusCodeProvider _responseCodeProvider;

        public ErrorResponseProvider(IErrorProvider errorProvider,
            IErrorMessageFormatter errorMessageFormatter,
            IHttpStatusCodeProvider responseCodeProvider,
            IErrorResponseProviderOptions options)
        {
            if (errorProvider == null) throw new ArgumentNullException(nameof(errorProvider));
            if (errorMessageFormatter == null) throw new ArgumentNullException(nameof(errorMessageFormatter));
            if (responseCodeProvider == null) throw new ArgumentNullException(nameof(responseCodeProvider));
            if (options == null) throw new ArgumentNullException(nameof(options));
            _errorProvider = errorProvider;
            _errorMessageFormatter = errorMessageFormatter;
            _responseCodeProvider = responseCodeProvider;
            _options = options;
        }

        public InternalErrorResponse GetErrors<TException>(TException exception)
            where TException : Exception
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            var userError = _errorProvider.ExtractErrorsFor(exception,
                ErrorProviderOptionsFactory.ExcludeDetailedErrors(_options));
            var message = _errorMessageFormatter.FormatErrorMessages(userError);
            var statusCode = _responseCodeProvider.GetHttpStatusCode(exception);

            if (_options.ErrorLevelOfDetails == LevelOfDetails.StandardMessage)
                return new InternalErrorResponse(message, statusCode);

            if (_options.ErrorLevelOfDetails == LevelOfDetails.UserErrors)
                return new InternalErrorResponse(message, statusCode, userError);

            var detailedError = _errorProvider.ExtractErrorsFor(exception,
                ErrorProviderOptionsFactory.IncludeDetailedErrors(_options));

            return new InternalErrorResponse(message, statusCode, userError, detailedError);
        }
    }
}