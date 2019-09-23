using Microsoft.AspNetCore.Builder;

namespace EasyApi.AspNetCore.Logging.SeriLog
{
    public static class LogUserIdentityMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserIdentityLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<LogUserIdentityMiddleware>();
            
            return app;
        }
    }
}