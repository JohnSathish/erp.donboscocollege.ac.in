using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace ERP.Api.Swagger;

/// <summary>
/// Swagger operation filter to properly handle file upload endpoints with [FromForm] IFormFile parameters
/// </summary>
public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var methodInfo = context.MethodInfo;
        
        // Find all [FromForm] IFormFile parameters
        var fileParameters = methodInfo.GetParameters()
            .Where(p => 
            {
                var hasFromForm = p.GetCustomAttributes(typeof(FromFormAttribute), false).Any();
                var isFormFile = p.ParameterType == typeof(IFormFile) || 
                                p.ParameterType == typeof(Microsoft.AspNetCore.Http.IFormFile);
                return hasFromForm && isFormFile;
            })
            .ToList();

        // If no file parameters, skip
        if (!fileParameters.Any())
        {
            return;
        }

        // Remove file parameters from the operation parameters list
        // (they'll be in RequestBody instead)
        if (operation.Parameters != null)
        {
            var paramsToRemove = operation.Parameters
                .Where(p => fileParameters.Any(fp => fp.Name == p.Name))
                .ToList();
            
            foreach (var param in paramsToRemove)
            {
                operation.Parameters.Remove(param);
            }
        }

        // Create or update RequestBody for multipart/form-data
        if (operation.RequestBody == null)
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Required = true,
                Content = new Dictionary<string, OpenApiMediaType>()
            };
        }

        // Ensure multipart/form-data content exists
        if (!operation.RequestBody.Content.ContainsKey("multipart/form-data"))
        {
            operation.RequestBody.Content["multipart/form-data"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>()
                }
            };
        }

        var multipartSchema = operation.RequestBody.Content["multipart/form-data"].Schema;
        if (multipartSchema.Properties == null)
        {
            multipartSchema.Properties = new Dictionary<string, OpenApiSchema>();
        }

        // Add each file parameter to the schema
        foreach (var param in fileParameters)
        {
            var paramName = param.Name ?? "file";
            
            multipartSchema.Properties[paramName] = new OpenApiSchema
            {
                Type = "string",
                Format = "binary",
                Description = $"Upload {paramName}"
            };
            
            // Mark as required if the parameter is not nullable
            if (!param.IsOptional)
            {
                multipartSchema.Required ??= new HashSet<string>();
                multipartSchema.Required.Add(paramName);
            }
        }

        // Also handle other [FromForm] parameters that are not IFormFile
        var otherFormParams = methodInfo.GetParameters()
            .Where(p => 
            {
                var hasFromForm = p.GetCustomAttributes(typeof(FromFormAttribute), false).Any();
                var isFormFile = p.ParameterType == typeof(IFormFile) || 
                                p.ParameterType == typeof(Microsoft.AspNetCore.Http.IFormFile);
                return hasFromForm && !isFormFile;
            })
            .ToList();

        foreach (var param in otherFormParams)
        {
            var paramName = param.Name ?? "value";
            
            // Determine schema type based on parameter type
            OpenApiSchema schema;
            if (param.ParameterType == typeof(string))
            {
                schema = new OpenApiSchema { Type = "string" };
            }
            else if (param.ParameterType == typeof(int) || param.ParameterType == typeof(int?))
            {
                schema = new OpenApiSchema { Type = "integer", Format = "int32" };
            }
            else if (param.ParameterType == typeof(bool) || param.ParameterType == typeof(bool?))
            {
                schema = new OpenApiSchema { Type = "boolean" };
            }
            else
            {
                schema = new OpenApiSchema { Type = "string" };
            }

            multipartSchema.Properties[paramName] = schema;

            if (!param.IsOptional)
            {
                multipartSchema.Required ??= new HashSet<string>();
                multipartSchema.Required.Add(paramName);
            }
        }
    }
}
