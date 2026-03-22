using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ERP.Api.Swagger;

/// <summary>
/// Parameter filter to prevent Swashbuckle from generating parameters for [FromForm] IFormFile
/// These will be handled by FileUploadOperationFilter in the RequestBody instead
/// </summary>
public class FileUploadParameterFilter : IParameterFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        if (context.ParameterInfo != null)
        {
            var hasFromForm = context.ParameterInfo.GetCustomAttributes(typeof(FromFormAttribute), false).Any();
            var isFormFile = context.ParameterInfo.ParameterType == typeof(IFormFile) || 
                            context.ParameterInfo.ParameterType == typeof(Microsoft.AspNetCore.Http.IFormFile);
            
            // If this is a [FromForm] IFormFile parameter, we need to prevent Swashbuckle
            // from trying to generate a parameter schema for it
            // The operation filter will handle it in the RequestBody instead
            if (hasFromForm && isFormFile)
            {
                // Set the schema to a simple type to avoid the error
                // The operation filter will override this with the correct RequestBody
                parameter.Schema = new Microsoft.OpenApi.Models.OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                };
            }
        }
    }
}

