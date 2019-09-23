using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyApi.ExceptionHandling.Domain;

namespace EasyApi.ExceptionHandling.Formatter
{
    public sealed class HierarchicalErrorMessageFormatter : IErrorMessageFormatter
    {
        private const int IndentFirstLevelBy = 0;
        private readonly IErrorMessageFormatterOptions _options;
        
        public HierarchicalErrorMessageFormatter(IErrorMessageFormatterOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _options = options;
        }

        public string FormatErrorMessages(Error error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));

            var errorMessages = ProcessError(error, IndentFirstLevelBy).ToList();
            if (errorMessages.Count == 1)
            {
                return errorMessages.Single();
            }

            var sb = new StringBuilder();
            foreach (var errorMessage in errorMessages)
            {
                sb.AppendLine(errorMessage);
            }

            return sb.ToString().TrimEnd();
        }

        private bool ContainsChildErrors(Error error)
        {
            return error.ChildErrors != null && error.ChildErrors.Any();
        }

        private List<string> ProcessError(Error error,
            int indentationLevel)
        {
            var nestedIndentationLevel = indentationLevel + 1;
            var indentBy = string.Concat(Enumerable.Repeat(_options.IndentBy, indentationLevel));
            var formattedErrorMessages = new List<string>
            {
                IndentText(error.Message, indentBy)
            };

            if (ContainsChildErrors(error))
            {
                foreach (var childError in error.ChildErrors)
                {
                    formattedErrorMessages.AddRange(ProcessError(childError, nestedIndentationLevel));
                }
            }
           
            return formattedErrorMessages;
        }

        private string IndentText(string message, string indentBy)
        {
            message = $"{indentBy}{message}";
            return message.Replace(Environment.NewLine, Environment.NewLine + indentBy);
        }
    }
}