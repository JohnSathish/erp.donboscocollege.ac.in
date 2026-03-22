using MediatR;
using ERP.Domain.Attendance.Entities;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Attendance.Commands.MarkAttendance;

public sealed record MarkAttendanceCommand(
    [Required] Guid SessionId,
    [Required] Guid PersonId,
    [Required] PersonType PersonType,
    [Required] AttendanceStatus Status,
    string? MarkedBy = null,
    string? DeviceId = null,
    string? DeviceType = null,
    string? Remarks = null) : IRequest<bool>;

