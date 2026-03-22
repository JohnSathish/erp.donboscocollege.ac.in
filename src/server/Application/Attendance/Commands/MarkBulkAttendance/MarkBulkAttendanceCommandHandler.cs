using ERP.Application.Attendance.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Attendance.Commands.MarkBulkAttendance;

public sealed class MarkBulkAttendanceCommandHandler : IRequestHandler<MarkBulkAttendanceCommand, bool>
{
    private readonly IAttendanceService _attendanceService;
    private readonly ILogger<MarkBulkAttendanceCommandHandler> _logger;

    public MarkBulkAttendanceCommandHandler(
        IAttendanceService attendanceService,
        ILogger<MarkBulkAttendanceCommandHandler> logger)
    {
        _attendanceService = attendanceService;
        _logger = logger;
    }

    public async Task<bool> Handle(MarkBulkAttendanceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _attendanceService.MarkBulkAttendanceAsync(
                request.SessionId,
                request.Items,
                request.MarkedBy,
                cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking bulk attendance for session {SessionId}.", request.SessionId);
            return false;
        }
    }
}

