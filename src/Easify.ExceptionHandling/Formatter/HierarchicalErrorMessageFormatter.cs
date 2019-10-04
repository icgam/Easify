// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easify.ExceptionHandling.Domain;

namespace Easify.ExceptionHandling.Formatter
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
            if (errorMessages.Count == 1) return errorMessages.Single();

            var sb = new StringBuilder();
            foreach (var errorMessage in errorMessages) sb.AppendLine(errorMessage);

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
                foreach (var childError in error.ChildErrors)
                    formattedErrorMessages.AddRange(ProcessError(childError, nestedIndentationLevel));

            return formattedErrorMessages;
        }

        private string IndentText(string message, string indentBy)
        {
            message = $"{indentBy}{message}";
            return message.Replace(Environment.NewLine, Environment.NewLine + indentBy);
        }
    }
}