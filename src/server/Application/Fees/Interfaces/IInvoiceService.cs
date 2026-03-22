using ERP.Domain.Fees.Entities;

namespace ERP.Application.Fees.Interfaces;

public interface IInvoiceService
{
    Task<Invoice> GenerateInvoiceFromFeeStructureAsync(
        Guid studentId,
        Guid feeStructureId,
        string academicYear,
        string? term = null,
        DateTime? dueDate = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default);

    Task<Invoice> GenerateCustomInvoiceAsync(
        Guid studentId,
        string academicYear,
        IReadOnlyCollection<InvoiceLineItem> lineItems,
        DateTime dueDate,
        string? term = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default);

    Task ApplyScholarshipsToInvoiceAsync(
        Guid invoiceId,
        CancellationToken cancellationToken = default);

    Task<string> GenerateInvoiceNumberAsync(
        string academicYear,
        CancellationToken cancellationToken = default);
}

public sealed record InvoiceLineItem(
    string Description,
    decimal UnitPrice,
    decimal Quantity = 1,
    Guid? FeeComponentId = null);

