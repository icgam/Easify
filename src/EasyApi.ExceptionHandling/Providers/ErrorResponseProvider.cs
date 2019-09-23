using System;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.Formatter;

namespace EasyApi.ExceptionHandling.Providers
{
    public sealed class ErrorResponseProvider : IErrorResponseProvider
    {
        private readonly IErrorMessageFormatter _errorMessageFormatter;
        private readonly IHttpStatusCodeProvider _responseCodeProvider;
        private readonly IErrorProvider _errorProvider;
        private readonly IErrorResponseProviderOptions _options;

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