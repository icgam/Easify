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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easify.Logging.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Easify.AspNetCore.ActionFilters
{
    public class ValidateModelStateActionFilter : IActionFilter, IAsyncActionFilter
    {
        private readonly ILogger _log;

        public ValidateModelStateActionFilter(ILogger<ValidateModelStateActionFilter> log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            ValidRequest(context);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (ValidRequest(context)) await next();
        }

        private bool ValidRequest(ActionExecutingContext context)
        {
            _log.LogInformation(() => $"Validating model for {context.ActionDescriptor.DisplayName} action ...");

            if (ModelIsValid(context))
            {
                _log.LogInformation(m =>
                    m("Model is valid. Executing {0} action ...", context.ActionDescriptor.DisplayName));
                return true;
            }

            if (ArgumentsAreNotSupplied(context))
            {
                var missingArguments = GetMissingArguments(context);
                _log.LogWarning(
                    () =>
                        $"Model validation for {context.ActionDescriptor.DisplayName} action has failed! Null arguments found!");

                _log.LogWarning(missingArguments);
                context.ModelState.AddModelError("ArgumentsMissing", missingArguments);
            }

            if (!context.ModelState.IsValid)
            {
                _log.LogWarning(
                    () => $"Model validation for {context.ActionDescriptor.DisplayName} action has failed!");
                var validationErrors = BuildValidationErrorsMessage(context);
                _log.LogWarning(
                    () => validationErrors);

                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
                return false;
            }

            return false;
        }

        private static string BuildValidationErrorsMessage(ActionExecutingContext context)
        {
            var errors = context.ModelState
                .Where(s => s.Value.Errors.Count > 0)
                .Select(s => new KeyValuePair<string, string>(s.Key, s.Value.Errors.First().ErrorMessage))
                .ToArray();

            var sb = new StringBuilder();
            sb.AppendLine("Validation errors listed below:");
            foreach (var error in errors) sb.AppendLine($"    {error.Key} -> {error.Value}");
            return sb.ToString();
        }

        private static bool ModelIsValid(ActionExecutingContext context)
        {
            return ArgumentsAreNotSupplied(context) == false && context.ModelState.IsValid;
        }

        private static bool ArgumentsAreNotSupplied(ActionExecutingContext context)
        {
            return context.ActionArguments != null && context.ActionArguments.Any(kv => kv.Value == null);
        }

        private string GetMissingArguments(ActionExecutingContext context)
        {
            var argumentNames =
                context.ActionArguments.Where(kv => kv.Value == null).Select(kv => kv.Key).ToList();

            var sb = new StringBuilder();

            sb.Append(argumentNames.Count == 1
                ? "1 argument is not supplied: "
                : $"{argumentNames.Count} arguments are not supplied: ");

            foreach (var argument in argumentNames)
                if (LastArgument(argument, argumentNames))
                    sb.AppendFormat("'{0}'.", argument);
                else
                    sb.AppendFormat("'{0}', ", argument);
            return sb.ToString();
        }

        private bool LastArgument(string argument, IList<string> argumentNames)
        {
            return argumentNames.Count == 1 || argumentNames.IndexOf(argument) == argument.Length - 1;
        }
    }
}
