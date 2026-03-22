using ERP.Application.Attendance.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Attendance.Queries.GetAttendanceSummary;

public sealed class GetAttendanceSummaryQueryHandler : IRequestHandler<GetAttendanceSummaryQuery, AttendanceSummaryDto?>
{
    private readonly IAttendanceService _attendanceService;
    private readonly ILogger<GetAttendanceSummaryQueryHandler> _logger;

    public GetAttendanceSummaryQueryHandler(
        IAttendanceService attendanceService,
        ILogger<GetAttendanceSummaryQueryHandler> logger)
    {
        _attendanceService = attendanceService;
        _logger = logger;
    }

    public async Task<AttendanceSummaryDto?> Handle(GetAttendanceSummaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.PersonType == Domain.Attendance.Entities.PersonType.Student)
            {
                if (string.IsNullOrWhiteSpace(request.AcademicYear))
                {
                    _logger.LogWarning("Academic year is required for student attendance summary.");
                    return null;
                }

                return await _attendanceService.GetStudentAttendanceSummaryAsync(
                    request.PersonId,
                    request.AcademicYear,
                    request.Term,
                    cancellationToken);
            }
            else if (request.PersonType == Domain.Attendance.Entities.PersonType.Staff)
            {
                if (!request.FromDate.HasValue || !request.ToDate.HasValue)
                {
                    _logger.LogWarning("FromDate and ToDate are required for staff attendance summary.");
                    return null;
                }

                return await _attendanceService.GetStaffAttendanceSummaryAsync(
                    request.PersonId,
                    request.FromDate.Value,
                    request.ToDate.Value,
                    cancellationToken);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving attendance summary for person {PersonId}.", request.PersonId);
            return null;
        }
    }
}

