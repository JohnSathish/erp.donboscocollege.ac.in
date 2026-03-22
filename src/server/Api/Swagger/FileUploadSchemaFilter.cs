using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ERP.Api.Swagger;

/// <summary>
/// Schema filter to handle IFormFile type mapping
/// This ensures IFormFile is always mapped to string/binary
/// </summary>
public class FileUploadSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(IFormFile) || context.Type == typeof(Microsoft.AspNetCore.Http.IFormFile))
        {
            schema.Type = "string";
            schema.Format = "binary";
            schema.Properties?.Clear();
        }
    }
}



