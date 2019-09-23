namespace EasyApi.AspNetCore.Bootstrap
{
    public interface ISetDetailsLevel :IConfigureRequestCorrelation
    {
        IConfigureRequestCorrelation UseStandardMessage();
        IConfigureRequestCorrelation UseUserErrors();
        IConfigureRequestCorrelation UseDetailedErrors();
    }
}