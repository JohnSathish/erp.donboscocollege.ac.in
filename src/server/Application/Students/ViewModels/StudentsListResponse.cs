namespace ERP.Application.Students.ViewModels;

public sealed record StudentsListResponse(
    IReadOnlyCollection<StudentDto> Students,
    int TotalCount,
    int Page,
    int PageSize);


