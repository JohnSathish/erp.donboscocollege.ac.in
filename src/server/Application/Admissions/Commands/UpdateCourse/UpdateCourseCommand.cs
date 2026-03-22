using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateCourse;

public sealed record UpdateCourseCommand(
    Guid Id,
    string Name,
    int CreditHours,
    Guid? ProgramId = null,
    string? Description = null,
    string? Prerequisites = null,
    string? UpdatedBy = null) : IRequest<bool>;









