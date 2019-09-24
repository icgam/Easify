// This software is part of the EasyApi framework
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

﻿using System;
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