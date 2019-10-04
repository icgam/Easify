// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using Easify.ExceptionHandling.Domain;
using Easify.ExceptionHandling.ErrorBuilder;
using Easify.ExceptionHandling.ErrorBuilder.Fluent;

namespace Easify.ExceptionHandling.ConfigurationBuilder
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