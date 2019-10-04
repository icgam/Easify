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
using System.Net;

namespace Easify.ExceptionHandling.Domain
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