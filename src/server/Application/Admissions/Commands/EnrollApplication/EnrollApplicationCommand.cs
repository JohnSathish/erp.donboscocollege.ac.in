using MediatR;

namespace ERP.Application.Admissions.Commands.EnrollApplication;

public sealed record EnrollApplicationCommand(
    Guid AccountId,
    string? EnrolledBy,
    string? Remarks,
    bool NotifyApplicant = true) : IRequest<EnrollApplicationResult>;

public sealed record EnrollApplicationResult(
    Guid AccountId,
    string ApplicationNumber,
    string FullName,
    DateTime EnrolledOnUtc);









