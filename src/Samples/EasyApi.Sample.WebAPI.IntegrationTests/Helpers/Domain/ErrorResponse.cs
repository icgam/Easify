using System.Collections.Generic;

namespace EasyApi.Sample.WebAPI.IntegrationTests.Helpers.Domain
{
    internal sealed class ErrorResponse
    {
      public string Message { get; set; }
        public IEnumerable<Error> UserErrors { get; set; } = new List<Error>();
        public IEnumerable<Error> RawErrors { get; set; } = new List<Error>();
    }
}