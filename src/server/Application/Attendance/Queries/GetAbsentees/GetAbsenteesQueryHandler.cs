using ERP.Application.Attendance.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Attendance.Queries.GetAbsentees;

public sealed class GetAbsenteesQueryHandler : IRequestHandler<GetAbsenteesQuery, IReadOnlyCollection<AbsenteeAlertDto>>
{
    private readonly IAttendanceService _attendanceService;
    private readonly ILogger<GetAbsenteesQueryHandler> _logger;

    public GetAbsenteesQueryHandler(
        IAttendanceService attendanceService,
        ILogger<GetAbsenteesQueryHandler> logger)
    {
        _attendanceService = attendanceService;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<AbsenteeAlertDto>> Handle(GetAbsenteesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _attendanceService.GetAbsenteesAsync(
                request.Date,
                request.ClassSectionId,
                request.CourseId,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving absentees for date {Date}.", request.Date);
            return Array.Empty<AbsenteeAlertDto>();
        }
    }
}

