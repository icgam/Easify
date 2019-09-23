using System;
using Microsoft.AspNetCore.Builder;

namespace EasyApi.AspNetCore.Health
{
    public static class DiagnosticsExtensions
    {
        public static IApplicationBuilder UseHealth(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<HealthMiddleware>();
        }
    }
}