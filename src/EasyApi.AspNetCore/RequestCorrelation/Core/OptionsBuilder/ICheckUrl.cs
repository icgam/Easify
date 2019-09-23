namespace EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder
{
    public interface ICheckUrl
    {
        bool UrlMatches(string url);
    }
}