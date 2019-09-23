using System;

namespace EasyApi.ExceptionHandling.ErrorBuilder.Fluent
{
    public interface ISetErrorBuilder<TException> where TException : Exception
    {
        IProvideErrorBuilder<TException> UseDefault();
        IProvideErrorBuilder<TException> Use(IErrorBuilder<TException> builder);
    }
}
