using System.Collections.Generic;
using EasyApi.AspNetCore.RequestCorrelation.Domain;
using EasyApi.Http;
using Microsoft.AspNetCore.Http;

namespace EasyApi.AspNetCore.RequestCorrelation
{
    public static class HttpRequestExtensions
    {
        public static string GetRequestId(this HttpRequest request)
        {
            if (request.Headers.ContainsKey(HttpHeaders.HttpCorrelationId))
                return request.Headers[HttpHeaders.HttpCorrelationId];

            if (request.Headers.ContainsKey(HttpHeaders.HttpRequestId))
                return request.Headers[HttpHeaders.HttpRequestId];

            throw new NotCorrelatedRequestException(
                new List<string> {HttpHeaders.HttpCorrelationId, HttpHeaders.HttpRequestId});
        }

        public static bool HasRequestId(this HttpRequest request)
        {
            return request.Headers.ContainsKey(HttpHeaders.HttpCorrelationId) ||
                   request.Headers.ContainsKey(HttpHeaders.HttpRequestId);
        }
    }
}