using System;
using System.Net;
using System.Threading.Tasks;
using EasyApi.Logging.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace EasyApi.AspNetCore.ActionFilters
{
    // TODO: Does it need to even be an action
    public class LoggingActionFilter : IActionFilter, IAsyncActionFilter
    {
        private readonly ILogger _log;

        public LoggingActionFilter(ILogger<LoggingActionFilter> log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            BeforeExecution(context);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            AfterExecution(context);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            BeforeExecution(context);
            var actionExecutedContext = await next();
            AfterExecution(actionExecutedContext);
        }

        private void BeforeExecution(ActionExecutingContext context)
        {
            _log.LogDebug(() => $"Entering {context.ActionDescriptor.DisplayName} method");

            if (_log.IsEnabled(LogLevel.Trace) == false)
                return;

            if (context.ActionArguments.Count <= 0)
                return;

            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value == null)
                {
                    _log.LogTrace(() => $"Method argument {argument.Key}: NULL");
                    continue;
                }

                if (_log.IsEnabled(LogLevel.Trace))
                {
                    _log.LogTrace("Method argument {ArgumentKey}: {@Value}", argument.Key, argument.Value);
                }
            }
        }

        private void AfterExecution(ActionExecutedContext context)
        {
            var responseCode = context.HttpContext.Response?.StatusCode ?? (int)HttpStatusCode.OK;
            _log.LogDebug(
                () => $"Exiting {context.ActionDescriptor.DisplayName} method. Returned status code {responseCode}.");
        }
    }
}