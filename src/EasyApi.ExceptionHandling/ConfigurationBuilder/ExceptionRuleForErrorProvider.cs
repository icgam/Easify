using System;
using System.Collections.Generic;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.ErrorBuilder;
using EasyApi.ExceptionHandling.ErrorBuilder.Fluent;

namespace EasyApi.ExceptionHandling.ConfigurationBuilder
{
    public sealed class ExceptionRuleForErrorProvider<TException> : IExceptionRule where TException : Exception
    {
        private readonly IErrorBuilder<TException> _errorBuilder;
        private readonly Type _exceptionType;
        private readonly Func<TException, bool> _predicate;

        public ExceptionRuleForErrorProvider() : this(ex => true)
        {
        }

        public ExceptionRuleForErrorProvider(Func<TException, bool> predicate) : 
            this(p => p.UseDefault(), predicate)
        {
        }

        public ExceptionRuleForErrorProvider(
            Func<ISetErrorBuilder<TException>, IProvideErrorBuilder<TException>> errorBuilderProvider) : this(
            errorBuilderProvider,
            ex => true)
        {
        }

        public ExceptionRuleForErrorProvider(
            Func<ISetErrorBuilder<TException>, IProvideErrorBuilder<TException>> errorBuilderProvider,
            Func<TException, bool> predicate)
        {
            if (errorBuilderProvider == null) throw new ArgumentNullException(nameof(errorBuilderProvider));
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            _exceptionType = typeof(TException);
            TypeFullName = _exceptionType.FullName;
            _errorBuilder = errorBuilderProvider(new ErrorBuilderConfiguration<TException>()).GetBuilder();
        }

        public bool CanHandle<TExceptionToCheck>(TExceptionToCheck exception)
            where TExceptionToCheck : Exception
        {
            return _exceptionType.IsInstanceOfType(exception) && _predicate(exception as TException);
        }

        public string TypeFullName { get; }

        public Error GetError<TExceptionToBuildErrorFor>(TExceptionToBuildErrorFor exception,
            IEnumerable<Error> internalErrors,
            bool includeSystemLevelExceptions)
            where TExceptionToBuildErrorFor : Exception
        {
            if (CanHandle(exception) == false)
                throw new ArgumentException(
                    $"'{exception.GetType().FullName}' can't be handled by this exception rule. Rules supports " +
                    $"'{typeof(TException).FullName}' and its derived types only!",
                    nameof(exception));

            var exceptionForBuilder = exception as TException;
            return _errorBuilder.Build(exceptionForBuilder, internalErrors, includeSystemLevelExceptions);
        }
    }
}