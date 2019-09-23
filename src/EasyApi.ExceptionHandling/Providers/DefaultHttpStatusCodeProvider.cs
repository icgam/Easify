using System;
using System.Linq;
using System.Net;
using EasyApi.ExceptionHandling.Domain;

namespace EasyApi.ExceptionHandling.Providers
{
    public sealed class DefaultHttpStatusCodeProvider : IHttpStatusCodeProvider
    {
        private readonly IErrorResponseProviderOptions _options;

        public DefaultHttpStatusCodeProvider(IErrorResponseProviderOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public HttpStatusCode GetHttpStatusCode<TException>(TException exception)
            where TException : Exception
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            return _options.RulesForExceptionHandling.Any(et => et.CanHandle(exception))
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.InternalServerError;
        }
    }
}