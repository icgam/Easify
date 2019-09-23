using System;
using System.Linq;
using System.Threading.Tasks;
using EasyApi.ExceptionHandling.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EasyApi.ExceptionHandling
{
    public sealed class ErrorHandlingMiddleware
    {
        private readonly (string header, string value)[] _defaultCorsHeaders =
        {
            ("Access-Control-Allow-Origin", "*"),
            ("Access-Control-Request-Method", "*"),
            ("Access-Control-Allow-Headers", "*")
        };

        private readonly IHostingEnvironment _env;
        private readonly IErrorResponseProvider _errorResponseProvider;
        private readonly ILogger<ErrorHandlingMiddleware> _log;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next, IHostingEnvironment env,
            IErrorResponseProvider errorResponseProvider,
            ILogger<ErrorHandlingMiddleware> log)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _errorResponseProvider =
                errorResponseProvider ?? throw new ArgumentNullException(nameof(errorResponseProvider));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = _errorResponseProvider.GetErrors(exception);

            if (_env.IsProduction())
                response = response.UserErrors.Any()
                    ? new InternalErrorResponse(response.Message, response.StatusCode, response.UserErrors.Single())
                    : new InternalErrorResponse(response.Message, response.StatusCode);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) response.StatusCode;

            foreach (var (header, value) in _defaultCorsHeaders) context.Response.Headers[header] = value;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse(response)));
        }
    }
}