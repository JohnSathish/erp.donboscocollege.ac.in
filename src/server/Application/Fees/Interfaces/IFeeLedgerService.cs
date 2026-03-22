using ERP.Domain.Fees.Entities;

namespace ERP.Application.Fees.Interfaces;

public interface IFeeLedgerService
{
    Task<FeeLedgerSummaryDto> GetStudentFeeLedgerAsync(
        Guid studentId,
        string? academicYear = null,
        CancellationToken cancellationToken = default);

    Task<AgingReportDto> GetAgingReportAsync(
        DateTime? asOfDate = null,
        Guid? studentId = null,
        CancellationToken cancellationToken = default);

    Task<FeeReconciliationDto> ReconcileFeesAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);
}

public sealed record FeeLedgerSummaryDto(
    Guid StudentId,
    string StudentNumber,
    string StudentName,
    decimal TotalInvoiced,
    decimal TotalPaid,
    decimal TotalRefunded,
    decimal OutstandingBalance,
    IReadOnlyCollection<InvoiceSummaryDto> Invoices,
    IReadOnlyCollection<PaymentSummaryDto> Payments);

public sealed record InvoiceSummaryDto(
    Guid InvoiceId,
    string InvoiceNumber,
    DateTime IssueDate,
    DateTime DueDate,
    decimal TotalAmount,
    decimal PaidAmount,
    decimal BalanceAmount,
    InvoiceStatus Status);

public sealed record PaymentSummaryDto(
    Guid PaymentId,
    string PaymentNumber,
    DateTime PaymentDate,
    decimal Amount,
    PaymentMethod PaymentMethod,
    PaymentStatus Status);

public sealed record AgingReportDto(
    DateTime AsOfDate,
    IReadOnlyCollection<AgingBucketDto> Buckets,
    decimal TotalOutstanding);

public sealed record AgingBucketDto(
    string BucketName, // e.g., "0-30 days", "31-60 days", "61-90 days", "90+ days"
    int DaysFrom,
    int DaysTo,
    decimal Amount,
    int InvoiceCount);

public sealed record FeeReconciliationDto(
    DateTime FromDate,
    DateTime ToDate,
    decimal TotalInvoiced,
    decimal TotalPaid,
    decimal TotalRefunded,
    decimal ExpectedBalance,
    decimal ActualBalance,
    decimal Variance,
    bool IsBalanced);

