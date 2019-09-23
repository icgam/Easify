using Microsoft.AspNetCore.Builder;

namespace EasyApi.AspNetCore.Logging.SeriLog
{
    public static class CorrelatedLogsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelatedLogs(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelatedLogsMiddleware>();
            return app;
        }
    }
}