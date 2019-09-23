using System;
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