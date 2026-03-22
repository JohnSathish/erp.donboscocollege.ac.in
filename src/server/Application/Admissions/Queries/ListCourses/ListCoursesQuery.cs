using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListCourses;

public sealed record ListCoursesQuery(
    int Page = 1,
    int PageSize = 50,
    bool? IsActive = null,
    Guid? ProgramId = null,
    string? SearchTerm = null) : IRequest<CoursesListResponse>;









