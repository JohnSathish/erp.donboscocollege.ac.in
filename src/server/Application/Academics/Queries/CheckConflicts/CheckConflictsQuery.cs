using MediatR;
using ERP.Application.Academics.Interfaces;

namespace ERP.Application.Academics.Queries.CheckConflicts;

public sealed record CheckConflictsQuery(
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    Guid? TeacherId = null,
    string? RoomNumber = null,
    string? Building = null,
    Guid? ExcludeSlotId = null) : IRequest<ConflictCheckResult>;

