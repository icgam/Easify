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