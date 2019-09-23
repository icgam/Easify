using System;
using EasyApi.ExceptionHandling.ErrorBuilder.Fluent;

namespace EasyApi.AspNetCore.Bootstrap
{
    public interface IHandleAdditionalException : ISetDetailsLevel
    {
        IHandleAdditionalException AndHandle<TThirdPartyBaseException>()
            where TThirdPartyBaseException : Exception;

        IHandleAdditionalException AndHandle<TThirdPartyBaseException>(Func<TThirdPartyBaseException, bool> predicate)
            where TThirdPartyBaseException : Exception;

        IHandleAdditionalException AndHandle<TThirdPartyBaseException>(
            Func<ISetErrorBuilder<TThirdPartyBaseException>, IProvideErrorBuilder<TThirdPartyBaseException>>
                errorBuilderProvider)
            where TThirdPartyBaseException : Exception;

        IHandleAdditionalException AndHandle<TThirdPartyBaseException>(
            Func<ISetErrorBuilder<TThirdPartyBaseException>, IProvideErrorBuilder<TThirdPartyBaseException>>
                errorBuilderProvider, Func<TThirdPartyBaseException, bool> predicate)
            where TThirdPartyBaseException : Exception;
    }
}