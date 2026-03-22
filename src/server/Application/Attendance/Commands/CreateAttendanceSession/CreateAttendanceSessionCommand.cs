using MediatR;
using ERP.Domain.Attendance.Entities;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Attendance.Commands.CreateAttendanceSession;

public sealed record CreateAttendanceSessionCommand(
    [Required] string SessionName,
    [Required] SessionType Type,
    [Required] DateTime SessionDate,
    [Required] string AcademicYear,
    Guid? ClassSectionId = null,
    Guid? CourseId = null,
    Guid? StaffShiftId = null,
    TimeOnly? StartTime = null,
    TimeOnly? EndTime = null,
    string? Term = null,
    string? Remarks = null,
    string? CreatedBy = null) : IRequest<Guid>;

