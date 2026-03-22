namespace ERP.Application.Admissions.ViewModels;

public sealed record FeeStructuresListResponse(
    IReadOnlyCollection<FeeStructureDto> FeeStructures,
    int TotalCount,
    int Page,
    int PageSize);









