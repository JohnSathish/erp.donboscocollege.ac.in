using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Queries.ListCourses;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListCourses;

public sealed class ListCoursesQueryHandler : IRequestHandler<ListCoursesQuery, CoursesListResponse>
{
    private readonly ICourseRepository _repository;

    public ListCoursesQueryHandler(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<CoursesListResponse> Handle(ListCoursesQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var (courses, totalCount) = await _repository.GetPagedAsync(
            page,
            pageSize,
            request.IsActive,
            request.ProgramId,
            request.SearchTerm,
            cancellationToken);

        var courseDtos = courses.Select(c => new CourseDto(
            c.Id,
            c.Code,
            c.Name,
            c.Description,
            c.ProgramId,
            c.Program?.Name,
            c.CreditHours,
            c.Prerequisites,
            c.IsActive,
            c.CreatedOnUtc,
            c.CreatedBy,
            c.UpdatedOnUtc,
            c.UpdatedBy)).ToList();

        return new CoursesListResponse(courseDtos, totalCount, page, pageSize);
    }
}









