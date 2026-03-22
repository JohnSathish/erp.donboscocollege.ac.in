namespace ERP.Application.Admissions.ViewModels;

public sealed record ProgramsListResponse(
    IReadOnlyCollection<ProgramDto> Programs,
    int TotalCount,
    int Page,
    int PageSize);









