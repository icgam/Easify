namespace EasyApi.Sample.WebAPI.Core.Mappings
{
    public interface IRateProvider
    {
        string GetRating(string assetId);
    }
}
