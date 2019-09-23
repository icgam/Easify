namespace EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder
{
    public interface ISetUrlFilter
    {
        ICheckUrl WhenStartsWith(string urlFragment);
        ICheckUrl WhenEndsWith(string urlFragment);
        ICheckUrl WhenContains(string urlFragment);
    }
}