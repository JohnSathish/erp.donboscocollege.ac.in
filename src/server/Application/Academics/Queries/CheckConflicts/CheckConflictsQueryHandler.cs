using ERP.Application.Academics.Interfaces;
using MediatR;

namespace ERP.Application.Academics.Queries.CheckConflicts;

public sealed class CheckConflictsQueryHandler : IRequestHandler<CheckConflictsQuery, ConflictCheckResult>
{
    private readonly ITimetableService _timetableService;

    public CheckConflictsQueryHandler(ITimetableService timetableService)
    {
        _timetableService = timetableService;
    }

    public async Task<ConflictCheckResult> Handle(CheckConflictsQuery request, CancellationToken cancellationToken)
    {
        return await _timetableService.CheckConflictsAsync(
            request.DayOfWeek,
            request.StartTime,
            request.EndTime,
            request.TeacherId,
            request.RoomNumber,
            request.Building,
            request.ExcludeSlotId,
            cancellationToken);
    }
}

