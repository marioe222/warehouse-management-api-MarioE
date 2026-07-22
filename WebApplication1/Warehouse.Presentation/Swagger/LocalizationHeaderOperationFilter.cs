using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Warehouse.Presentation.Swagger;

public class LocalizationHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<IOpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Description = "Culture code (en, ar, fr)",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String,
                Default = "en",
                Enum = new List<JsonNode>
                {
                    JsonValue.Create("en")!,
                    JsonValue.Create("ar")!,
                    JsonValue.Create("fr")!
                }
            }
        });
    }
}