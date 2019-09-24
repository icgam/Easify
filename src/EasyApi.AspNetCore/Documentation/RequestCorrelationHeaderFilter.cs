// This software is part of the EasyApi framework
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
using EasyApi.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EasyApi.AspNetCore.Documentation
{
    public sealed class RequestCorrelationHeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Parameters = operation.Parameters ?? new List<IParameter>();
            operation.Parameters.Add(new NonBodyParameter
            {
                Name = HttpHeaders.HttpRequestId,
                In = "header",
                Type = "string",
                Description = "Correlation-ID",
                Required = true,
                Default = Guid.NewGuid().ToString()
            });
        }
    }
}