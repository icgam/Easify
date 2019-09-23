using System;
using System.Collections.Generic;
using System.Linq;
using EasyApi.ExceptionHandling.ConfigurationBuilder.Fluent;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.ErrorBuilder.Fluent;
using EasyApi.ExceptionHandling.Formatter;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.ExceptionHandling.ConfigurationBuilder
{
    public sealed class GlobalErrorHandlerConfigurationBuilder : IHandleApplicationException,
        IHandleAdditionalExceptions, IProvideGenericError
    {
        private const string GenericErrorMessage =
            "Unexpected error has occured. Please try again or contact IT support.";

        private const string GenericErrorType = "UnknownException";

        private const string DefaultIndentationSymbol = " ";

        private readonly IDictionary<string, IExceptionRule> _rulesDictionary =
            new Dictionary<string, IExceptionRule>();

        private readonly IServiceCollection _services;
        private LevelOfDetails _errorLevelOfDetail;
        private Error _genericError;
        private string _indentationSymbol;

        public GlobalErrorHandlerConfigurationBuilder(IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _genericError = new Error(GenericErrorMessage, typeof(Exception).Name);
            _indentationSymbol = DefaultIndentationSymbol;
        }

        public IHandleAdditionalExceptions AndHandle<TThirdPartyBaseException>()
            where TThirdPartyBaseException : Exception
        {
            var type = typeof(TThirdPartyBaseException);
            if (_rulesDictionary.ContainsKey(type.FullName) == false)
                _rulesDictionary.Add(type.FullName, new ExceptionRuleForErrorProvider<TThirdPartyBaseException>());

            return this;
        }

        public IHandleAdditionalExceptions AndHandle<TThirdPartyBaseException>(
            Func<TThirdPartyBaseException, bool> predicate) where TThirdPartyBaseException : Exception
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            var type = typeof(TThirdPartyBaseException);
            if (_rulesDictionary.ContainsKey(type.FullName) == false)
                _rulesDictionary.Add(type.FullName,
                    new ExceptionRuleForErrorProvider<TThirdPartyBaseException>(predicate));

            return this;
        }

        public IHandleAdditionalExceptions AndHandle<TThirdPartyBaseException>(
            Func<ISetErrorBuilder<TThirdPartyBaseException>, IProvideErrorBuilder<TThirdPartyBaseException>>
                errorBuilderProvider, Func<TThirdPartyBaseException, bool> predicate)
            where TThirdPartyBaseException : Exception
        {
            if (errorBuilderProvider == null) throw new ArgumentNullException(nameof(errorBuilderProvider));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            var type = typeof(TThirdPartyBaseException);

            if (_rulesDictionary.ContainsKey(type.FullName) == false)
                _rulesDictionary.Add(type.FullName,
                    new ExceptionRuleForErrorProvider<TThirdPartyBaseException>(errorBuilderProvider, predicate));

            return this;
        }

        public IProvideGenericError UseStandardMessage()
        {
            _errorLevelOfDetail = LevelOfDetails.StandardMessage;
            return this;
        }

        public IProvideGenericError UseUserErrors()
        {
            _errorLevelOfDetail = LevelOfDetails.UserErrors;
            return this;
        }

        public IProvideGenericError UseDetailedErrors()
        {
            _errorLevelOfDetail = LevelOfDetails.DetailedErrors;
            return this;
        }

        public IProvideCustomMessageFormatter IndentMessagesUsing(string indentationSymbol)
        {
            if (string.IsNullOrWhiteSpace(indentationSymbol))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(indentationSymbol));
            _indentationSymbol = indentationSymbol;
            return this;
        }

        public IBuildErrorProviderOptions FormatMessageUsing<TFormatter>()
            where TFormatter : class, IErrorMessageFormatter
        {
            _services.AddTransient<IErrorMessageFormatter, TFormatter>();
            return this;
        }

        public IProvideIndentationSymbol UseGenericError()
        {
            _genericError = new Error(GenericErrorMessage, GenericErrorType);

            return this;
        }

        public IProvideIndentationSymbol UseGenericError(string errorMessage, string errorType)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorMessage));
            if (string.IsNullOrWhiteSpace(errorType))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorType));

            _genericError = new Error(errorMessage, errorType);

            return this;
        }

        public IErrorResponseProviderOptions Build()
        {
            var options = new ErrorResponseProviderOptions
            {
                GenericError = _genericError,
                RulesForExceptionHandling = _rulesDictionary.Select(e => e.Value).ToList(),
                ErrorLevelOfDetails = _errorLevelOfDetail,
                IndentBy = _indentationSymbol
            };

            return options;
        }

        public IHandleAdditionalExceptions AndHandle<TThirdPartyBaseException>(
            Func<ISetErrorBuilder<TThirdPartyBaseException>, IProvideErrorBuilder<TThirdPartyBaseException>>
                errorBuilderProvider) where TThirdPartyBaseException : Exception
        {
            if (errorBuilderProvider == null) throw new ArgumentNullException(nameof(errorBuilderProvider));
            var type = typeof(TThirdPartyBaseException);

            if (_rulesDictionary.ContainsKey(type.FullName) == false)
                _rulesDictionary.Add(type.FullName,
                    new ExceptionRuleForErrorProvider<TThirdPartyBaseException>(errorBuilderProvider));

            return this;
        }

        public IHandleAdditionalExceptions Handle<TApplicationBaseException>()
            where TApplicationBaseException : Exception
        {
            _rulesDictionary.Add(typeof(TApplicationBaseException).FullName,
                new ExceptionRuleForErrorProvider<TApplicationBaseException>());
            return this;
        }

        private sealed class ErrorResponseProviderOptions : IErrorResponseProviderOptions
        {
            public string IndentBy { get; set; }
            public Error GenericError { get; set; }
            public IReadOnlyList<IExceptionRule> RulesForExceptionHandling { get; set; }
            public LevelOfDetails ErrorLevelOfDetails { get; set; }
        }
    }
}