using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ERP.Application.Admissions.Commands.SaveApplicantApplicationDraft;
using ERP.Application.Admissions.Commands.SubmitApplicantApplication;
using ERP.Application.Admissions.Commands.UploadDocument;
using ERP.Application.Admissions.Commands.AcceptAdmissionOffer;
using ERP.Application.Admissions.Commands.RejectAdmissionOffer;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Queries.GetApplicantApplicationDraft;
using ERP.Application.Admissions.Queries.GetApplicantOffer;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/applicant-applications")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Applicant")]
public class ApplicantApplicationsController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<ApplicantApplicationDraftResponse>> GetDraft(CancellationToken cancellationToken)
    {
        if (!TryGetAccountId(out var accountId))
        {
            return Unauthorized();
        }

        var draft = await mediator.Send(new GetApplicantApplicationDraftQuery(accountId), cancellationToken);
        return Ok(draft);
    }

    [HttpPost("me")]
    public async Task<ActionResult<ApplicantApplicationDraftResponse>> SaveDraft(
        [FromBody] ApplicantApplicationDraftDto payload,
        CancellationToken cancellationToken)
    {
        if (!TryGetAccountId(out var accountId))
        {
            return Unauthorized();
        }

        var result = await mediator.Send(new SaveApplicantApplicationDraftCommand(accountId, payload), cancellationToken);
        return Ok(result);
    }

    [HttpPost("me/submit")]
    public async Task<IActionResult> Submit(
        [FromBody] ApplicantApplicationDraftDto payload,
        CancellationToken cancellationToken)
    {
        if (!TryGetAccountId(out var accountId))
        {
            return Unauthorized();
        }

        var pdf = await mediator.Send(new SubmitApplicantApplicationCommand(accountId, payload), cancellationToken);
        return File(pdf.Content, pdf.ContentType, pdf.FileName);
    }

    [HttpPost("me/documents/{documentType}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<UploadDocumentResult>> UploadDocument(
        string documentType,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        if (!TryGetAccountId(out var accountId))
        {
            return Unauthorized();
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file provided." });
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        var fileData = memoryStream.ToArray();

        var command = new UploadDocumentCommand(
            accountId,
            documentType,
            file.FileName,
            file.ContentType,
            fileData);

        try
        {
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("me/offer")]
    public async Task<ActionResult<AdmissionOfferDto>> GetOffer(CancellationToken cancellationToken)
    {
        if (!TryGetAccountId(out var accountId))
        {
            return Unauthorized();
        }

        var query = new GetApplicantOfferQuery(accountId);
        var result = await mediator.Send(query, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("me/offer/accept")]
    public async Task<ActionResult<AcceptAdmissionOfferResult>> AcceptOffer(CancellationToken cancellationToken)
    {
        if (!TryGetAccountId(out var accountId))
        {
            return Unauthorized();
        }

        // Get offer for this account
        var offerQuery = new GetApplicantOfferQuery(accountId);
        var offer = await mediator.Send(offerQuery, cancellationToken);
        if (offer == null)
        {
            return NotFound(new { message = "No admission offer found." });
        }

        var command = new AcceptAdmissionOfferCommand(offer.Id);
        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("me/offer/reject")]
    public async Task<ActionResult<RejectAdmissionOfferResult>> RejectOffer(
        [FromBody] RejectOfferRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        if (!TryGetAccountId(out var accountId))
        {
            return Unauthorized();
        }

        // Get offer for this account
        var offerQuery = new GetApplicantOfferQuery(accountId);
        var offer = await mediator.Send(offerQuery, cancellationToken);
        if (offer == null)
        {
            return NotFound(new { message = "No admission offer found." });
        }

        var command = new RejectAdmissionOfferCommand(offer.Id, request?.Reason);
        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    private bool TryGetAccountId(out Guid accountId)
    {
        var accountIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(accountIdClaim, out accountId);
    }
}

public sealed record RejectOfferRequest(string? Reason = null);

