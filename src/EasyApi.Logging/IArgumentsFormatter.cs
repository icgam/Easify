using System.Collections.Generic;

namespace EasyApi.Logging
{
    public interface IArgumentsFormatter
    {
        string FormatArguments(object[] arguments);
        string FormatArguments(List<object> arguments);
        string FormatArgument(object argument);
    }
}