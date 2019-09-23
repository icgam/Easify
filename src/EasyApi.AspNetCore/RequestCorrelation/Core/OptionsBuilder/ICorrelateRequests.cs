namespace EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder
{
    public interface ICorrelateRequests
    {
        IBuildOptions EnforceCorrelation();
        IBuildOptions AutoCorrelateRequests();
    }
}