using System;
using EasyApi.ExceptionHandling.ErrorBuilder.Fluent;

namespace EasyApi.ExceptionHandling.ConfigurationBuilder.Fluent
{
    public interface IHandleAdditionalExceptions : ISetDetailsLevel, IProvideGenericError
    {
        IHandleAdditionalExceptions AndHandle<TThirdPartyBaseException>()
            where TThirdPartyBaseException : Exception;

        IHandleAdditionalExceptions AndHandle<TThirdPartyBaseException>(Func<TThirdPartyBaseException, bool> predicate)
            where TThirdPartyBaseException : Exception;

        IHandleAdditionalExceptions AndHandle<TThirdPartyBaseException>(
            Func<ISetErrorBuilder<TThirdPartyBaseException>, IProvideErrorBuilder<TThirdPartyBaseException>>
                errorBuilderProvider)
            where TThirdPartyBaseException : Exception;

        IHandleAdditionalExceptions AndHandle<TThirdPartyBaseException>(
            Func<ISetErrorBuilder<TThirdPartyBaseException>, IProvideErrorBuilder<TThirdPartyBaseException>>
                errorBuilderProvider, Func<TThirdPartyBaseException, bool> predicate)
            where TThirdPartyBaseException : Exception;
    }
}