using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.ViewModels;

public sealed record PaymentDto(
    Guid AccountId,
    string ApplicationNumber,
    string FullName,
    string Email,
    string MobileNumber,
    bool IsPaymentCompleted,
    string? PaymentOrderId,
    string? PaymentTransactionId,
    decimal? PaymentAmount,
    DateTime? PaymentCompletedOnUtc,
    DateTime CreatedOnUtc,
    ApplicationStatus ApplicationStatus);

