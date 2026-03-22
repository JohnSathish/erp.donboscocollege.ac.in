using MediatR;
using ERP.Domain.Fees.Entities;

namespace ERP.Application.Fees.Queries.GetInvoice;

public sealed record GetInvoiceQuery(Guid InvoiceId) : IRequest<InvoiceDto?>;

public sealed record InvoiceDto(
    Guid Id,
    Guid StudentId,
    string StudentNumber,
    string StudentName,
    string InvoiceNumber,
    Guid? FeeStructureId,
    string AcademicYear,
    string? Term,
    DateTime IssueDate,
    DateTime DueDate,
    decimal SubTotal,
    decimal DiscountAmount,
    decimal ScholarshipAmount,
    decimal TaxAmount,
    decimal TotalAmount,
    decimal PaidAmount,
    decimal BalanceAmount,
    InvoiceStatus Status,
    string? Remarks,
    IReadOnlyCollection<InvoiceLineDto> Lines);

public sealed record InvoiceLineDto(
    Guid Id,
    Guid? FeeComponentId,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal Amount,
    int DisplayOrder);
