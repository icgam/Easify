using System.Collections.Generic;

namespace EasyApi.Sample.WebAPI.IntegrationTests.Helpers.Domain
{
    internal sealed class Error
    {
        public string Message { get; set; }
        public string ErrorType { get; set; }
        public IEnumerable<Error> ChildErrors { get; set; }
    }
}