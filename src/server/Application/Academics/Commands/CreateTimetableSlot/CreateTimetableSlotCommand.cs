using ERP.Application.Academics.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Academics.Commands.CreateTimetableSlot;

public sealed record CreateTimetableSlotCommand(
    [Required] Guid ClassSectionId,
    [Required] DayOfWeek DayOfWeek,
    [Required] TimeOnly StartTime,
    [Required] TimeOnly EndTime,
    Guid? TeacherId = null,
    string? TeacherName = null,
    string? RoomNumber = null,
    string? Building = null,
    string? Remarks = null,
    string? CreatedBy = null) : IRequest<CreateTimetableSlotResult>;

public sealed record CreateTimetableSlotResult(
    Guid SlotId,
    bool Success,
    string? ErrorMessage = null,
    ConflictCheckResult? Conflicts = null);

