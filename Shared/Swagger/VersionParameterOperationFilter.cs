using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Contracts.Swagger;

public class VersionParameterOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
        operation.Parameters.Remove(versionParameter);
    }
}