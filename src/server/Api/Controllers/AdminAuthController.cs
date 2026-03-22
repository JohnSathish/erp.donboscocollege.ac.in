using ERP.Application.Admissions.Commands.LoginAdmin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/auth/admin")]
public class AdminAuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginAdminResponse>> Login(
        [FromBody] LoginAdminRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new LoginAdminCommand(request.Username, request.Password);
            var result = await mediator.Send(command, cancellationToken);

            return Ok(MapToResponse(result));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    private static LoginAdminResponse MapToResponse(LoginAdminResult result) =>
        new(
            result.Token,
            result.ExpiresAtUtc,
            result.UniqueId,
            result.Email,
            result.FullName);
}

public sealed record LoginAdminRequest(string Username, string Password);

public sealed record LoginAdminResponse(
    string Token,
    DateTime ExpiresAtUtc,
    string UniqueId,
    string Email,
    string FullName);














