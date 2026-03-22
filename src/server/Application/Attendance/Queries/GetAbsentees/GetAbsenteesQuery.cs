using MediatR;
using ERP.Application.Attendance.Interfaces;

namespace ERP.Application.Attendance.Queries.GetAbsentees;

public sealed record GetAbsenteesQuery(
    DateTime Date,
    Guid? ClassSectionId = null,
    Guid? CourseId = null) : IRequest<IReadOnlyCollection<AbsenteeAlertDto>>;

