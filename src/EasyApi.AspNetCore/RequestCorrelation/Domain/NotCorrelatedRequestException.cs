using System;
using System.Collections.Generic;

namespace EasyApi.AspNetCore.RequestCorrelation.Domain
{
    public sealed class NotCorrelatedRequestException : Exception
    {
        public NotCorrelatedRequestException(IEnumerable<string> supportedHeaders) : base((string) GetMessage(supportedHeaders))
        {
        }

        private static string GetMessage(IEnumerable<string> supportedHeaders)
        {
            return
                $"Request is NOT correlated! One of the {string.Join(",", supportedHeaders)} headers with correlation ID must be present!";
        }
    }
}