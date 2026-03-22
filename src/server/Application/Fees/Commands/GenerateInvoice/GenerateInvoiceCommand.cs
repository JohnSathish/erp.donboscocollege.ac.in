using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Fees.Commands.GenerateInvoice;

public sealed record GenerateInvoiceCommand(
    Guid StudentId,
    [Required] string AcademicYear,
    Guid? FeeStructureId = null,
    string? Term = null,
    DateTime? DueDate = null,
    IReadOnlyCollection<InvoiceLineItemRequest>? CustomLineItems = null,
    string? CreatedBy = null) : IRequest<GenerateInvoiceResult>;

public sealed record InvoiceLineItemRequest(
    string Description,
    decimal UnitPrice,
    decimal Quantity = 1,
    Guid? FeeComponentId = null);

public sealed record GenerateInvoiceResult(
    Guid InvoiceId,
    string InvoiceNumber,
    decimal TotalAmount,
    bool Success,
    string? ErrorMessage = null);

