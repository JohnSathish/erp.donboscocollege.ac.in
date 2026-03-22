namespace ERP.Application.Admissions.ViewModels;

public sealed record CoursesListResponse(
    IReadOnlyCollection<CourseDto> Courses,
    int TotalCount,
    int Page,
    int PageSize);









