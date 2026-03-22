using MediatR;

using ERP.Domain.Fees.Entities;

namespace ERP.Application.Fees.Queries.GetStudentInvoices;

public sealed record GetStudentInvoicesQuery(
    Guid StudentId,
    string? AcademicYear = null) : IRequest<IReadOnlyCollection<InvoiceSummaryDto>>;

public sealed record InvoiceSummaryDto(
    Guid InvoiceId,
    string InvoiceNumber,
    DateTime IssueDate,
    DateTime DueDate,
    decimal TotalAmount,
    decimal PaidAmount,
    decimal BalanceAmount,
    InvoiceStatus Status);

