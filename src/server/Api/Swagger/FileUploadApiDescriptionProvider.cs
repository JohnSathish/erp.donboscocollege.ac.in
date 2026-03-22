using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Linq;
using System.Reflection;

namespace ERP.Api.Swagger;

/// <summary>
/// API Description Provider to modify API descriptions for [FromForm] IFormFile parameters
/// This runs before Swashbuckle tries to generate parameters, preventing the error
/// </summary>
public class FileUploadApiDescriptionProvider : IApiDescriptionProvider
{
    public int Order => -1000; // Run early

    public void OnProvidersExecuting(ApiDescriptionProviderContext context)
    {
        // No-op - we only modify on execution
    }

    public void OnProvidersExecuted(ApiDescriptionProviderContext context)
    {
        foreach (var apiDescription in context.Results)
        {
            // Find [FromForm] IFormFile parameters
            var actionDescriptor = apiDescription.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            if (actionDescriptor == null) continue;

            var parameters = actionDescriptor.MethodInfo.GetParameters();
            var fileParameters = parameters
                .Where(p => 
                {
                    var hasFromForm = p.GetCustomAttributes(typeof(FromFormAttribute), false).Any();
                    var isFormFile = p.ParameterType == typeof(IFormFile) || 
                                    p.ParameterType == typeof(Microsoft.AspNetCore.Http.IFormFile);
                    return hasFromForm && isFormFile;
                })
                .ToList();

            if (!fileParameters.Any()) continue;

            // Remove these parameters completely from the API description
            // Swashbuckle will not try to generate schemas for parameters that don't exist
            // The operation filter will add them to RequestBody instead
            var paramsToRemove = apiDescription.ParameterDescriptions
                .Where(pd => fileParameters.Any(fp => fp.Name == pd.Name))
                .ToList();

            foreach (var param in paramsToRemove)
            {
                apiDescription.ParameterDescriptions.Remove(param);
            }
        }
    }
}

