using System;
using System.Collections.Generic;
using System.Net;

namespace EasyApi.ExceptionHandling.Domain
{
    public sealed class InternalErrorResponse
    {
        public InternalErrorResponse(string message, HttpStatusCode statusCode, Error userError, Error rawError)
        {
            if (userError == null) throw new ArgumentNullException(nameof(userError));
            if (rawError == null) throw new ArgumentNullException(nameof(rawError));
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            if (!Enum.IsDefined(typeof(HttpStatusCode), statusCode))
                throw new ArgumentOutOfRangeException(nameof(statusCode),
                    "Value should be defined in the HttpStatusCode enum.");
            Message = message;
            StatusCode = statusCode;
            UserErrors = new List<Error>
            {
                userError
            };
            RawErrors = new List<Error>
            {
                rawError
            };
        }

        public InternalErrorResponse(string message, HttpStatusCode statusCode)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            if (!Enum.IsDefined(typeof(HttpStatusCode), statusCode))
                throw new ArgumentOutOfRangeException(nameof(statusCode),
                    "Value should be defined in the HttpStatusCode enum.");
            Message = message;
            StatusCode = statusCode;
        }

        public InternalErrorResponse(string message, HttpStatusCode statusCode, Error userError)
        {
            if (userError == null) throw new ArgumentNullException(nameof(userError));
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            if (!Enum.IsDefined(typeof(HttpStatusCode), statusCode))
                throw new ArgumentOutOfRangeException(nameof(statusCode),
                    "Value should be defined in the HttpStatusCode enum.");
            Message = message;
            StatusCode = statusCode;
            UserErrors = new List<Error>
            {
                userError
            };
        }

        public string Message { get; }
        public HttpStatusCode StatusCode { get; }
        public IEnumerable<Error> UserErrors { get; } = new List<Error>();
        public IEnumerable<Error> RawErrors { get; } = new List<Error>();
    }
}