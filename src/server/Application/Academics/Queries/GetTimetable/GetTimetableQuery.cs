using MediatR;
using ERP.Application.Academics.Interfaces;

namespace ERP.Application.Academics.Queries.GetTimetable;

public sealed record GetTimetableQuery(
    Guid? ClassSectionId = null,
    Guid? TeacherId = null,
    string? RoomNumber = null,
    string? Building = null,
    DayOfWeek? DayOfWeek = null) : IRequest<IReadOnlyCollection<TimetableSlotDto>>;

public sealed record TimetableSlotDto(
    Guid Id,
    Guid ClassSectionId,
    string SectionName,
    string? CourseName,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string? RoomNumber,
    string? Building,
    Guid? TeacherId,
    string? TeacherName,
    string? Remarks);

