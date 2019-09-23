using System;

namespace EasyApi.Sample.WebAPI.Domain
{
    public abstract class ThirdPartyPluginException : Exception
    {
        protected ThirdPartyPluginException(string message) : base(message)
        {
        }
    }
}