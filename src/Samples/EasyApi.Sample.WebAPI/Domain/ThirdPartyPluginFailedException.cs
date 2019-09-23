namespace EasyApi.Sample.WebAPI.Domain
{
    public sealed class ThirdPartyPluginFailedException : ThirdPartyPluginException
    {
        public ThirdPartyPluginFailedException() : base("Third party plugin has failed!")
        {
        }
    }
}