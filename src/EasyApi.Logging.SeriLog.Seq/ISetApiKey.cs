namespace EasyApi.Logging.SeriLog.Seq
{
    public interface ISetApiKey
    {
        IControlLogLevel WithApiKey(string apiKey);
    }
}