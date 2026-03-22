using MediatR;

namespace ERP.Application.Admissions.Commands.ConfirmAdmissionFeePayment;

/// <summary>
/// Marks admission fee as paid for the direct-admission path (admin / payment callback).
/// </summary>
public sealed record ConfirmAdmissionFeePaymentCommand(
    Guid AccountId,
    string? PerformedBy,
    bool NotifyApplicant
) : IRequest<ConfirmAdmissionFeePaymentResult>;

public sealed record ConfirmAdmissionFeePaymentResult(Guid AccountId, string Status);
