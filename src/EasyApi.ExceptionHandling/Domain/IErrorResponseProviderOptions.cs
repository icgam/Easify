using System.Collections.Generic;
using EasyApi.ExceptionHandling.ConfigurationBuilder;
using EasyApi.ExceptionHandling.Formatter;

namespace EasyApi.ExceptionHandling.Domain
{
    public interface IErrorResponseProviderOptions : IErrorMessageFormatterOptions
    {
        Error GenericError { get; }
        IReadOnlyList<IExceptionRule> RulesForExceptionHandling { get; }
        LevelOfDetails ErrorLevelOfDetails { get; }
    }
}
