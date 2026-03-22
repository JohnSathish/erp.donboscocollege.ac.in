using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetPaymentReport;

public sealed record GetPaymentReportQuery(
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    bool? IsPaymentCompleted = null) : IRequest<PaymentReportDto>;

public sealed record PaymentReportDto(
    DateTime GeneratedOnUtc,
    DateTime? FromDate,
    DateTime? ToDate,
    PaymentReportSummary Summary,
    IReadOnlyCollection<PaymentReportItemDto> Items);

public sealed record PaymentReportSummary(
    int TotalPayments,
    int PaidCount,
    int PendingCount,
    decimal TotalRevenue,
    decimal AveragePaymentAmount,
    decimal MinPaymentAmount,
    decimal MaxPaymentAmount);

public sealed record PaymentReportItemDto(
    string ApplicationNumber,
    string FullName,
    string Email,
    string MobileNumber,
    bool IsPaymentCompleted,
    decimal? PaymentAmount,
    string? PaymentTransactionId,
    DateTime? PaymentCompletedOnUtc,
    DateTime CreatedOnUtc,
    string ApplicationStatus);











