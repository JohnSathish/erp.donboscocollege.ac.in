using ERP.Domain.Fees.Entities;

namespace ERP.Application.Fees.Interfaces;

public interface IPaymentService
{
    Task<PaymentResult> ProcessPaymentAsync(
        Guid invoiceId,
        decimal amount,
        PaymentMethod paymentMethod,
        DateTime paymentDate,
        string? paymentGateway = null,
        string? transactionId = null,
        string? referenceNumber = null,
        string? remarks = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default);

    Task<Receipt> GenerateReceiptAsync(
        Guid paymentId,
        string? createdBy = null,
        CancellationToken cancellationToken = default);

    Task<string> GeneratePaymentNumberAsync(
        string academicYear,
        CancellationToken cancellationToken = default);

    Task<string> GenerateReceiptNumberAsync(
        string academicYear,
        CancellationToken cancellationToken = default);
}

public sealed record PaymentResult(
    Payment Payment,
    Receipt? Receipt,
    bool Success,
    string? ErrorMessage = null);

