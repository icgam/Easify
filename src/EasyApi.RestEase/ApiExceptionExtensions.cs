using System.Net;
using RestEase;

namespace EasyApi.RestEase
{
    public static class ApiExceptionExtensions
    {
        private const int UnprocessableEntity = 422;
        
        public static bool ClientError(this ApiException f)
        {
            return f.StatusCode == HttpStatusCode.BadRequest ||
                   f.StatusCode == HttpStatusCode.Unauthorized ||
                   f.StatusCode == HttpStatusCode.Forbidden ||
                   f.StatusCode == HttpStatusCode.NotFound ||
                   f.StatusCode == HttpStatusCode.MethodNotAllowed ||
                   f.StatusCode == HttpStatusCode.NotAcceptable ||
                   f.StatusCode == HttpStatusCode.RequestTimeout ||
                   f.StatusCode == HttpStatusCode.Conflict ||
                   f.StatusCode == HttpStatusCode.Gone ||
                   f.StatusCode == HttpStatusCode.UnsupportedMediaType ||
                   f.StatusCode == HttpStatusCode.ExpectationFailed ||
                   f.StatusCode == HttpStatusCode.ProxyAuthenticationRequired ||
                   (int)f.StatusCode == UnprocessableEntity;
        }
    }
}