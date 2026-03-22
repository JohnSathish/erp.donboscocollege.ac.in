using ERP.Application.Students.Queries.ListStudents;
using ERP.Application.Students.ViewModels;
using MediatR;

namespace ERP.Application.Students.Queries.ListStudents;

public sealed record ListStudentsQuery(
    int Page = 1,
    int PageSize = 50,
    bool? IsActive = null,
    Guid? ProgramId = null,
    string? AcademicYear = null,
    string? SearchTerm = null) : IRequest<StudentsListResponse>;


