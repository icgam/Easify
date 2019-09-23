using EasyApi.ExceptionHandling.Formatter;

namespace EasyApi.ExceptionHandling.ConfigurationBuilder.Fluent
{
    public interface IProvideCustomMessageFormatter : IBuildErrorProviderOptions
    {
        IBuildErrorProviderOptions FormatMessageUsing<TFormatter>()
            where TFormatter : class, IErrorMessageFormatter;
    }
}