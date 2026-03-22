using ERP.Application.Attendance.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Attendance.Commands.CreateAttendanceSession;

public sealed class CreateAttendanceSessionCommandHandler : IRequestHandler<CreateAttendanceSessionCommand, Guid>
{
    private readonly IAttendanceService _attendanceService;
    private readonly ILogger<CreateAttendanceSessionCommandHandler> _logger;

    public CreateAttendanceSessionCommandHandler(
        IAttendanceService attendanceService,
        ILogger<CreateAttendanceSessionCommandHandler> logger)
    {
        _attendanceService = attendanceService;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateAttendanceSessionCommand request, CancellationToken cancellationToken)
    {
        var sessionId = await _attendanceService.CreateSessionAsync(
            request.SessionName,
            request.Type,
            request.SessionDate,
            request.AcademicYear,
            request.ClassSectionId,
            request.CourseId,
            request.StaffShiftId,
            request.StartTime,
            request.EndTime,
            request.Term,
            request.Remarks,
            request.CreatedBy,
            cancellationToken);

        _logger.LogInformation("Created attendance session {SessionId} for {SessionDate}.", sessionId, request.SessionDate);
        return sessionId;
    }
}

