using System.Collections.Generic;
using EasyApi.ExceptionHandling.ConfigurationBuilder;

namespace EasyApi.ExceptionHandling.Domain
{
    public interface IErrorProviderOptions
    {
        Error GenericError { get; }
        IReadOnlyList<IExceptionRule> ExceptionsToHandle { get; }
        bool IncludeSystemLevelExceptions { get; }
    }
}