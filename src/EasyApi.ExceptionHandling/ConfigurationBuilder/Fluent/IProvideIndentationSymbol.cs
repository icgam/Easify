namespace EasyApi.ExceptionHandling.ConfigurationBuilder.Fluent
{
    public interface IProvideIndentationSymbol : IProvideCustomMessageFormatter
    {
        IProvideCustomMessageFormatter IndentMessagesUsing(string indentationSymbol);
    }
}