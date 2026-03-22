using ERP.Application.Attendance.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Attendance.Commands.MarkAttendance;

public sealed class MarkAttendanceCommandHandler : IRequestHandler<MarkAttendanceCommand, bool>
{
    private readonly IAttendanceService _attendanceService;
    private readonly ILogger<MarkAttendanceCommandHandler> _logger;

    public MarkAttendanceCommandHandler(
        IAttendanceService attendanceService,
        ILogger<MarkAttendanceCommandHandler> logger)
    {
        _attendanceService = attendanceService;
        _logger = logger;
    }

    public async Task<bool> Handle(MarkAttendanceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _attendanceService.MarkAttendanceAsync(
                request.SessionId,
                request.PersonId,
                request.PersonType,
                request.Status,
                request.MarkedBy,
                request.DeviceId,
                request.DeviceType,
                request.Remarks,
                cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking attendance for person {PersonId} in session {SessionId}.",
                request.PersonId, request.SessionId);
            return false;
        }
    }
}

