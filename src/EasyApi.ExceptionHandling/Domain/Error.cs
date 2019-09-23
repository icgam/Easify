using System;
using System.Collections.Generic;

namespace EasyApi.ExceptionHandling.Domain
{
    public sealed class Error
    {
        public Error() {}

        public Error(string message, string errorType) : this(message, errorType, new List<Error>())
        {
        }

        public Error(string message, string errorType, Error childError) : this(message, errorType, new List<Error> { childError })
        {
            if (childError == null) throw new ArgumentNullException(nameof(childError));
        }

        public Error(string message, string errorType, IEnumerable<Error> childErrors)
        {
            if (childErrors == null) throw new ArgumentNullException(nameof(childErrors));
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            if (string.IsNullOrWhiteSpace(errorType))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorType));
            Message = message;
            ErrorType = errorType;
            ChildErrors = childErrors;
        }

        public string Message { get; set;  }
        public string ErrorType { get; set;  }
        public IEnumerable<Error> ChildErrors { get; set; }
    }
}