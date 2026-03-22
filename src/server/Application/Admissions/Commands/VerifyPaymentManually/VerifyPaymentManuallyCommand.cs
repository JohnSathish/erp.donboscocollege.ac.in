using MediatR;

namespace ERP.Application.Admissions.Commands.VerifyPaymentManually;

public sealed record VerifyPaymentManuallyCommand(
    Guid AccountId,
    string TransactionId,
    decimal Amount,
    string? VerifiedBy,
    string? Remarks) : IRequest<VerifyPaymentManuallyResult>;

public sealed record VerifyPaymentManuallyResult(Guid AccountId);











