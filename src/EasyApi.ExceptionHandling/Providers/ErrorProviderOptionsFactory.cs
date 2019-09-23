using System.Collections.Generic;
using EasyApi.ExceptionHandling.ConfigurationBuilder;
using EasyApi.ExceptionHandling.Domain;

namespace EasyApi.ExceptionHandling.Providers
{
    public sealed class ErrorProviderOptionsFactory : IErrorProviderOptions
    {
        public Error GenericError { get; }
        public IReadOnlyList<IExceptionRule> ExceptionsToHandle { get; }
        public bool IncludeSystemLevelExceptions { get; }

        private ErrorProviderOptionsFactory(IErrorResponseProviderOptions options, bool includeSystemLevelExceptions)
        {
            GenericError = options.GenericError;
            ExceptionsToHandle = options.RulesForExceptionHandling;
            IncludeSystemLevelExceptions = includeSystemLevelExceptions;
        }

        public static IErrorProviderOptions IncludeDetailedErrors(IErrorResponseProviderOptions options)
        {
            return new ErrorProviderOptionsFactory(options, true);
        }
        public static IErrorProviderOptions ExcludeDetailedErrors(IErrorResponseProviderOptions options)
        {
            return new ErrorProviderOptionsFactory(options, false);
        }
    }
}