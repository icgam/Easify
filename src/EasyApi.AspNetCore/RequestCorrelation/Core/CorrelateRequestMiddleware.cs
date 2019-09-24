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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyApi.AspNetCore.RequestCorrelation.Domain;
using EasyApi.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace EasyApi.AspNetCore.RequestCorrelation.Core
{
    public class CorrelateRequestMiddleware
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly RequestCorrelationOptions _correlationOptions;
        private readonly RequestDelegate _next;

        public CorrelateRequestMiddleware(RequestDelegate next, RequestCorrelationOptions correlationOptions,
            ICorrelationIdProvider correlationIdProvider)
        {
            _next = next;
            _correlationOptions = correlationOptions ?? throw new ArgumentNullException(nameof(correlationOptions));
            _correlationIdProvider =
                correlationIdProvider ?? throw new ArgumentNullException(nameof(correlationIdProvider));
        }

        public async Task Invoke(HttpContext context)
        {
            if (RequestNeedsCorrelation(context))
                context = CorrelateRequest(context);

            await _next(context);
        }

        private HttpContext CorrelateRequest(HttpContext context)
        {
            string correlationId;

            if (RequestIsCorrelated(context))
            {
                correlationId = context.Request.GetRequestId();
            }
            else
            {
                if (_correlationOptions.ForceCorrelation)
                    throw new NotCorrelatedRequestException(
                        new List<string> {HttpHeaders.HttpCorrelationId, HttpHeaders.HttpRequestId});

                correlationId = _correlationIdProvider.GenerateId();
                context.Request.Headers.Add(HttpHeaders.HttpRequestId, correlationId);
            }

            context.Response.OnStarting(state =>
            {
                ((HttpContext) state).Response.Headers.Add(HttpHeaders.HttpRequestId, correlationId);
                return Task.FromResult(0);
            }, context);

            return context;
        }

        private bool RequestNeedsCorrelation(HttpContext context)
        {
            return !_correlationOptions.UrlsToIgnoreRules.Any(u => u.UrlMatches(context.Request.GetDisplayUrl()));
        }

        private bool RequestIsCorrelated(HttpContext context)
        {
            return context.Request.Headers.ContainsKey(HttpHeaders.HttpCorrelationId) ||
                   context.Request.Headers.ContainsKey(HttpHeaders.HttpRequestId);
        }
    }
}