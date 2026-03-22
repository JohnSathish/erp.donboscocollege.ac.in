using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ERP.Application.Admissions.Queries.GetApplicantDashboard;
using ERP.Application.Admissions.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/applicant-portal")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Applicant")]
public class ApplicantPortalController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<ApplicantDashboardDto>> GetDashboard(CancellationToken cancellationToken)
    {
        var accountIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(accountIdClaim, out var accountId))
        {
            return Unauthorized();
        }

        var dashboard = await mediator.Send(new GetApplicantDashboardQuery(accountId), cancellationToken);
        return dashboard is null ? NotFound() : Ok(dashboard);
    }
}







