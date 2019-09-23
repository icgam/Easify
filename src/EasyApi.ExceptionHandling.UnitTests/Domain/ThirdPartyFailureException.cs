namespace EasyApi.ExceptionHandling.UnitTests.Domain
{
    public sealed class ThirdPartyFailureException : ThirdPartyLibraryExceptionBase
    {
        public ThirdPartyFailureException() : base("message")
        {
        }

        public ThirdPartyFailureException(string message) : base(message)
        {
        }
    }
}