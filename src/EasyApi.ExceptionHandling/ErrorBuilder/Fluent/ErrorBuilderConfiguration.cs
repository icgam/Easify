using System;

namespace EasyApi.ExceptionHandling.ErrorBuilder.Fluent
{
    public sealed class ErrorBuilderConfiguration<TException> : ISetErrorBuilder<TException>,
        IProvideErrorBuilder<TException> where TException : Exception
    {
        private IErrorBuilder<TException> _builder = new DefaultErrorBuilder<TException>();

        public IErrorBuilder<TException> GetBuilder()
        {
            return _builder;
        }

        public IProvideErrorBuilder<TException> UseDefault()
        {
            _builder = new DefaultErrorBuilder<TException>();
            return this;
        }

        public IProvideErrorBuilder<TException> Use(IErrorBuilder<TException> builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            return this;
        }
    }
}