namespace EasyApi.Logging.SeriLog.Loggly
{
    public interface ISetCustomerToken
    {
        IConfigureLogBuffer WithCustomerToken(string customerToken);
    }
}