using MediatR;
using ERP.Application.Attendance.Interfaces;
using ERP.Domain.Attendance.Entities;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Attendance.Commands.MarkBulkAttendance;

public sealed record MarkBulkAttendanceCommand(
    [Required] Guid SessionId,
    [Required] IReadOnlyCollection<BulkAttendanceItem> Items,
    string? MarkedBy = null) : IRequest<bool>;

