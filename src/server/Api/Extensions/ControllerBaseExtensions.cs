using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Extensions;

public static class ControllerBaseExtensions
{
    /// <summary>
    /// Gets the current authenticated user's identifier (email or username) from JWT claims.
    /// Returns "System" if no user is authenticated.
    /// </summary>
    public static string GetCurrentUserId(this ControllerBase controller)
    {
        var user = controller.HttpContext.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            return "System";
        }

        // Try to get email first (preferred)
        var email = user.FindFirstValue(ClaimTypes.Email) 
                   ?? user.FindFirstValue("email");
        
        if (!string.IsNullOrWhiteSpace(email))
        {
            return email;
        }

        // Fall back to name identifier
        var nameId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue("sub")
                    ?? user.FindFirstValue("name");
        
        return !string.IsNullOrWhiteSpace(nameId) ? nameId : "System";
    }

    /// <summary>
    /// Gets the current authenticated user's full name from JWT claims.
    /// Returns null if not available.
    /// </summary>
    public static string? GetCurrentUserName(this ControllerBase controller)
    {
        var user = controller.HttpContext.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        return user.FindFirstValue(ClaimTypes.Name)
               ?? user.FindFirstValue("name")
               ?? user.FindFirstValue("fullName");
    }
}

