using System;

namespace EasyApi.AspNetCore.Bootstrap
{
    public interface IConfigureApplicationBaseException
    {
        IHandleAdditionalException HandleApplicationException<TApplicationBaseException>()
            where TApplicationBaseException : Exception;
    }
}