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
using System.Net;
using System.Threading.Tasks;
using Easify.Logging.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Easify.AspNetCore.ActionFilters
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
                    _log.LogTrace("Method argument {ArgumentKey}: {@Value}", argument.Key, argument.Value);
            }
        }

        private void AfterExecution(ActionExecutedContext context)
        {
            var responseCode = context.HttpContext.Response?.StatusCode ?? (int) HttpStatusCode.OK;
            _log.LogDebug(
                () => $"Exiting {context.ActionDescriptor.DisplayName} method. Returned status code {responseCode}.");
        }
    }
}