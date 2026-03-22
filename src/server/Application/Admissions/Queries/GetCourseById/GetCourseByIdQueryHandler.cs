using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Queries.GetCourseById;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetCourseById;

public sealed class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto?>
{
    private readonly ICourseRepository _repository;

    public GetCourseByIdQueryHandler(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<CourseDto?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (course == null)
        {
            return null;
        }

        return new CourseDto(
            course.Id,
            course.Code,
            course.Name,
            course.Description,
            course.ProgramId,
            course.Program?.Name,
            course.CreditHours,
            course.Prerequisites,
            course.IsActive,
            course.CreatedOnUtc,
            course.CreatedBy,
            course.UpdatedOnUtc,
            course.UpdatedBy);
    }
}









