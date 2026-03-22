using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateOnlineApplicationStatus;

public sealed record UpdateOnlineApplicationStatusCommand(
    Guid AccountId,
    ApplicationStatus Status,
    string? UpdatedBy,
    string? Remarks,
    EntranceExamDetails? EntranceExam,
    bool NotifyApplicant,
    DateTime? PaymentDeadlineUtc = null) : IRequest<OnlineApplicationStatusUpdatedResult>;

public sealed record EntranceExamDetails(
    DateTime? ScheduledOnUtc,
    string? Venue,
    string? Instructions);

public sealed record OnlineApplicationStatusUpdatedResult(Guid AccountId);




