namespace EasyApi.Sample.WebAPI.Core.Mappings
{
    public sealed class DummyRateProvider : IRateProvider
    {
        public string GetRating(string assetId)
        {
            return "AAA";
        }
    }
}