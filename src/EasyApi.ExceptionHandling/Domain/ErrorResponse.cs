using System.Collections.Generic;

namespace EasyApi.ExceptionHandling.Domain
{
    public sealed class ErrorResponse
    {
        public ErrorResponse(InternalErrorResponse errorResponse)
        {
            Message = errorResponse.Message;
            UserErrors = errorResponse.UserErrors;
            RawErrors = errorResponse.RawErrors;
        }
        public ErrorResponse()
        {}

        public string Message { get; set; }
        public IEnumerable<Error> UserErrors { get; set; }
        public IEnumerable<Error> RawErrors { get; set; }
    }
}