using System.Threading.Tasks;
using EasyApi.AspNetCore.RequestCorrelation;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace EasyApi.AspNetCore.Logging.SeriLog
{
    public class CorrelatedLogsMiddleware
    {
        private const string CorrelationIdKey = "CorrelationId";
        private readonly RequestDelegate _next;

        public CorrelatedLogsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var requestId = "Not-Correlated";
            if (context.Request.HasRequestId())
            {
                requestId = context.Request.GetRequestId();
            }

            using (LogContext.PushProperty(CorrelationIdKey, requestId))
            {
                return _next(context);
            }
        }
    }
}