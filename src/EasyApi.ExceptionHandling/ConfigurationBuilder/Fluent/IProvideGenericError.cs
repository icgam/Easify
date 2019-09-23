namespace EasyApi.ExceptionHandling.ConfigurationBuilder.Fluent
{
    public interface IProvideGenericError : IProvideIndentationSymbol
    {
        IProvideIndentationSymbol UseGenericError(string errorMessage, string errorType);
        IProvideIndentationSymbol UseGenericError();
    }
}