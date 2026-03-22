using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateApplicantStatus;

public sealed record UpdateApplicantStatusCommand(
    Guid ApplicantId,
    ApplicationStatus Status,
    string? UpdatedBy,
    string? Remarks,
    EntranceExamDetails? EntranceExam,
    bool NotifyApplicant) : IRequest<ApplicantStatusUpdatedResult>;

public sealed record EntranceExamDetails(
    DateTime? ScheduledOnUtc,
    string? Venue,
    string? Instructions);

public sealed record ApplicantStatusUpdatedResult(Guid ApplicantId);






