namespace EasyApi.AspNetCore.RequestCorrelation.Core
{
    public interface ICorrelationIdProvider
    {
        string GenerateId();
    }
}