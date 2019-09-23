using EasyApi.ExceptionHandling.Domain;

namespace EasyApi.ExceptionHandling.Formatter
{
    public interface IErrorMessageFormatter
    {
        string FormatErrorMessages(Error error);
    }
}