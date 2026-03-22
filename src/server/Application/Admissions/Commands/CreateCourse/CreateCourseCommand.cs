using MediatR;

namespace ERP.Application.Admissions.Commands.CreateCourse;

public sealed record CreateCourseCommand(
    string Code,
    string Name,
    int CreditHours,
    Guid? ProgramId = null,
    string? Description = null,
    string? Prerequisites = null,
    string? CreatedBy = null) : IRequest<Guid>;









