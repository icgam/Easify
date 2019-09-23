using System;

namespace EasyApi.ExceptionHandling.ErrorBuilder.Fluent
{
    public interface IProvideErrorBuilder<TException> where TException : Exception
    {
        IErrorBuilder<TException> GetBuilder();
    }
}