using ERP.Application.Admissions.Queries.GetPublishedSelectionList;
using ERP.Application.Admissions.ViewModels;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/admissions/public")]
public sealed class PublicAdmissionsController(IMediator mediator) : ControllerBase
{
    /// <summary>Published selection lists (First / Second / Third) for the website.</summary>
    [HttpGet("selection-list")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<PublishedSelectionListEntryDto>>> GetPublishedSelectionList(
        [FromQuery] AdmissionSelectionListRound? round,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPublishedSelectionListQuery(round), cancellationToken);
        return Ok(result);
    }
}
