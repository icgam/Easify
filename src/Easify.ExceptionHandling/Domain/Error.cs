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

namespace Easify.ExceptionHandling.Domain
{
    public sealed class Error
    {
        public Error()
        {
        }

        public Error(string message, string errorType) : this(message, errorType, new List<Error>())
        {
        }

        public Error(string message, string errorType, Error childError) : this(message, errorType,
            new List<Error> {childError})
        {
            if (childError == null) throw new ArgumentNullException(nameof(childError));
        }

        public Error(string message, string errorType, IEnumerable<Error> childErrors)
        {
            if (childErrors == null) throw new ArgumentNullException(nameof(childErrors));
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            if (string.IsNullOrWhiteSpace(errorType))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorType));
            Message = message;
            ErrorType = errorType;
            ChildErrors = childErrors;
        }

        public string Message { get; set; }
        public string ErrorType { get; set; }
        public IEnumerable<Error> ChildErrors { get; set; }
    }
}