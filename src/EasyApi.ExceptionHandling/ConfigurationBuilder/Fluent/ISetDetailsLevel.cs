namespace EasyApi.ExceptionHandling.ConfigurationBuilder.Fluent
{
    public interface ISetDetailsLevel
    {
        IProvideGenericError UseStandardMessage();
        IProvideGenericError UseUserErrors();
        IProvideGenericError UseDetailedErrors();
    }
}