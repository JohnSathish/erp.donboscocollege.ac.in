using ERP.Application.Fees.Commands.GenerateInvoice;
using ERP.Application.Fees.Commands.ProcessPayment;
using ERP.Application.Fees.Commands.CreateRefundRequest;
using ERP.Application.Fees.Commands.ApproveRefund;
using ERP.Application.Fees.Commands.ProcessRefund;
using ERP.Application.Fees.Commands.CreateScholarship;
using ERP.Application.Fees.Commands.PublishInvoice;
using ERP.Application.Fees.Queries.GetInvoice;
using ERP.Application.Fees.Queries.GetStudentInvoices;
using ERP.Application.Fees.Queries.GetStudentFeeLedger;
using ERP.Application.Fees.Queries.GetAgingReport;
using ERP.Application.Fees.Interfaces;
using ERP.Domain.Fees.Entities;
using InvoiceSummaryDto = ERP.Application.Fees.Queries.GetStudentInvoices.InvoiceSummaryDto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeesController(IMediator mediator) : ControllerBase
{
    [HttpPost("invoices")]
    [Authorize(Roles = "Admin,FinanceOfficer,AcademicAdmin")]
    public async Task<ActionResult<GenerateInvoiceResult>> GenerateInvoice(
        [FromBody] GenerateInvoiceRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new GenerateInvoiceCommand(
            request.StudentId,
            request.AcademicYear,
            request.FeeStructureId,
            request.Term,
            request.DueDate,
            request.CustomLineItems,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetInvoice), new { id = result.InvoiceId }, result);
    }

    [HttpGet("invoices/{id:guid}")]
    [Authorize(Roles = "Admin,FinanceOfficer,AcademicAdmin,Student,Parent")]
    public async Task<ActionResult<InvoiceDto>> GetInvoice(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetInvoiceQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("students/{studentId:guid}/invoices")]
    [Authorize(Roles = "Admin,FinanceOfficer,AcademicAdmin,Student,Parent")]
    public async Task<ActionResult<IReadOnlyCollection<InvoiceSummaryDto>>> GetStudentInvoices(
        Guid studentId,
        [FromQuery] string? academicYear = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetStudentInvoicesQuery(studentId, academicYear);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("invoices/{id:guid}/publish")]
    [Authorize(Roles = "Admin,FinanceOfficer")]
    public async Task<ActionResult> PublishInvoice(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new PublishInvoiceCommand(id, User?.Identity?.Name);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : BadRequest("Cannot publish invoice. It may not be in draft status or may not have line items.");
    }

    [HttpPost("payments")]
    [Authorize(Roles = "Admin,FinanceOfficer")]
    public async Task<ActionResult<ProcessPaymentResult>> ProcessPayment(
        [FromBody] ProcessPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new ProcessPaymentCommand(
            request.InvoiceId,
            request.Amount,
            request.PaymentMethod,
            request.PaymentDate,
            request.PaymentGateway,
            request.TransactionId,
            request.ReferenceNumber,
            request.ChequeNumber,
            request.ChequeDate,
            request.BankName,
            request.Remarks,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetInvoice), new { id = request.InvoiceId }, result);
    }

    [HttpPost("refunds")]
    [Authorize(Roles = "Admin,FinanceOfficer")]
    public async Task<ActionResult<Guid>> CreateRefundRequest(
        [FromBody] CreateRefundRequestRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateRefundRequestCommand(
            request.PaymentId,
            request.Amount,
            request.Reason,
            request.ReasonDetails,
            request.Remarks,
            User?.Identity?.Name);

        try
        {
            var refundId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetInvoice), new { id = request.PaymentId }, refundId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("refunds/{id:guid}/approve")]
    [Authorize(Roles = "Admin,FinanceOfficer")]
    public async Task<ActionResult> ApproveRefund(
        Guid id,
        [FromBody] ApproveRefundRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        var command = new ApproveRefundCommand(id, request?.Remarks, User?.Identity?.Name);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("refunds/{id:guid}/process")]
    [Authorize(Roles = "Admin,FinanceOfficer")]
    public async Task<ActionResult> ProcessRefund(
        Guid id,
        [FromBody] ProcessRefundRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        var command = new ProcessRefundCommand(id, request?.Remarks, User?.Identity?.Name);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("scholarships")]
    [Authorize(Roles = "Admin,FinanceOfficer")]
    public async Task<ActionResult<Guid>> CreateScholarship(
        [FromBody] CreateScholarshipRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateScholarshipCommand(
            request.StudentId,
            request.ScholarshipName,
            request.Type,
            request.AcademicYear,
            request.EffectiveFrom,
            request.EffectiveTo,
            request.Percentage,
            request.FixedAmount,
            request.Description,
            request.SponsorName,
            request.ApprovalReference,
            request.Remarks,
            User?.Identity?.Name);

        try
        {
            var scholarshipId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetInvoice), new { id = request.StudentId }, scholarshipId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("students/{studentId:guid}/ledger")]
    [Authorize(Roles = "Admin,FinanceOfficer,AcademicAdmin,Student,Parent")]
    public async Task<ActionResult<FeeLedgerSummaryDto>> GetStudentFeeLedger(
        Guid studentId,
        [FromQuery] string? academicYear = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetStudentFeeLedgerQuery(studentId, academicYear);
        var result = await mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("aging-report")]
    [Authorize(Roles = "Admin,FinanceOfficer")]
    public async Task<ActionResult<AgingReportDto>> GetAgingReport(
        [FromQuery] DateTime? asOfDate = null,
        [FromQuery] Guid? studentId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAgingReportQuery(asOfDate, studentId);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("reconcile")]
    [Authorize(Roles = "Admin,FinanceOfficer")]
    public async Task<ActionResult<FeeReconciliationDto>> ReconcileFees(
        [FromBody] ReconcileFeesRequest request,
        [FromServices] IFeeLedgerService feeLedgerService,
        CancellationToken cancellationToken = default)
    {
        var result = await feeLedgerService.ReconcileFeesAsync(
            request.FromDate,
            request.ToDate,
            cancellationToken);
        return Ok(result);
    }
}

// Request DTOs
public sealed record GenerateInvoiceRequest(
    Guid StudentId,
    Guid? FeeStructureId = null,
    string AcademicYear = "",
    string? Term = null,
    DateTime? DueDate = null,
    IReadOnlyCollection<InvoiceLineItemRequest>? CustomLineItems = null);

public sealed record ProcessPaymentRequest(
    Guid InvoiceId,
    decimal Amount,
    PaymentMethod PaymentMethod,
    DateTime? PaymentDate = null,
    string? PaymentGateway = null,
    string? TransactionId = null,
    string? ReferenceNumber = null,
    string? ChequeNumber = null,
    DateTime? ChequeDate = null,
    string? BankName = null,
    string? Remarks = null);

public sealed record CreateRefundRequestRequest(
    Guid PaymentId,
    decimal Amount,
    RefundReason Reason,
    string? ReasonDetails = null,
    string? Remarks = null);

public sealed record ApproveRefundRequest(
    string? Remarks = null);

public sealed record ProcessRefundRequest(
    string? Remarks = null);

public sealed record CreateScholarshipRequest(
    Guid StudentId,
    string ScholarshipName,
    ScholarshipType Type,
    string AcademicYear,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo = null,
    decimal? Percentage = null,
    decimal? FixedAmount = null,
    string? Description = null,
    string? SponsorName = null,
    string? ApprovalReference = null,
    string? Remarks = null);

public sealed record ReconcileFeesRequest(
    DateTime FromDate,
    DateTime ToDate);

