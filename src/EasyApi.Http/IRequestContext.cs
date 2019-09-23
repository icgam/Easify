namespace EasyApi.Http
{
    public interface IRequestContext : IOperationContext
    {
        string CorrelationId { get; }
    }
}