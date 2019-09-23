using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace EasyApi.AspNetCore.Logging.SeriLog
{
    public sealed class LogUserIdentityMiddleware
    {
        private const string UserNameKey = "UserName";
        private const string AnonymousUser = "Anonymous";

        private readonly RequestDelegate _next;

        public LogUserIdentityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var user = AnonymousUser;

            if (context.User.Identity.IsAuthenticated)
                user = context.User.Identity.Name;

            using (LogContext.PushProperty(UserNameKey, user))
            {
                return _next(context);
            }
        }
    }
}