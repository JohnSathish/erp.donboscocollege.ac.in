using MediatR;

namespace ERP.Application.Admissions.Commands.GrantDirectAdmission;

public sealed record GrantDirectAdmissionCommand(
    Guid AccountId,
    string? PerformedBy,
    bool NotifyApplicant
) : IRequest<GrantDirectAdmissionResult>;

public sealed record GrantDirectAdmissionResult(
    Guid AccountId,
    string PaymentUrl,
    decimal ClassXiiPercentage,
    string Status);
