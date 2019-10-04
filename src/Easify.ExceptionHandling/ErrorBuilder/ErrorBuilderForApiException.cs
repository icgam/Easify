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
using EasyApi.ExceptionHandling.Domain;
using Newtonsoft.Json;
using RestEase;

namespace EasyApi.ExceptionHandling.ErrorBuilder
{
    // TODO: Should be renamed
    public sealed class ErrorBuilderForApiException : IErrorBuilder<ApiException>
    {
        private const string MessageKey = "\"Message\"";
        private const string UserErrorsKey = "\"UserErrors\"";
        private const string RawErrorsKey = "\"RawErrors\"";

        public Error Build(ApiException exception, IEnumerable<Error> internalErrors, bool includeSystemLevelExceptions)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            if (internalErrors == null) throw new ArgumentNullException(nameof(internalErrors));
            var typeName = exception.GetType().FullName;

            if (exception.HasContent)
            {
                var originalError = BuildOriginalError(exception, internalErrors, includeSystemLevelExceptions,
                    typeName);
                return new Error(exception.Message, typeName, originalError);
            }

            return new Error(exception.Message, typeName, internalErrors);
        }

        private Error BuildOriginalError(ApiException exception, IEnumerable<Error> internalErrors,
            bool includeSystemLevelExceptions,
            string typeName)
        {
            if (ContainsUnknownError(exception))
                return new Error(exception.Content, typeName, internalErrors);

            var childErrors = internalErrors as IList<Error> ?? internalErrors.ToList();
            try
            {
                var response = JsonConvert.DeserializeObject<ErrorResponse>(exception.Content);
                var errors = childErrors.ToList();
                errors.AddRange(GetRelevantErrors(includeSystemLevelExceptions, response));
                return new Error(response.Message, typeName, errors);
            }
            catch
            {
                return new Error(exception.Content, typeName, childErrors);
            }
        }

        private IEnumerable<Error> GetRelevantErrors(bool includeSystemLevelExceptions, ErrorResponse response)
        {
            if (includeSystemLevelExceptions && response.RawErrors.Any())
                return response.RawErrors;
            return response.UserErrors;
        }

        private bool ContainsUnknownError(ApiException exception)
        {
            return exception.Content.IndexOf(MessageKey, StringComparison.CurrentCultureIgnoreCase) == -1 ||
                   exception.Content.IndexOf(UserErrorsKey, StringComparison.CurrentCultureIgnoreCase) == -1 ||
                   exception.Content.IndexOf(RawErrorsKey, StringComparison.CurrentCultureIgnoreCase) == -1;
        }
    }
}