using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListFeeStructures;

public sealed record ListFeeStructuresQuery(
    int Page = 1,
    int PageSize = 50,
    bool? IsActive = null,
    Guid? ProgramId = null,
    string? AcademicYear = null,
    string? SearchTerm = null) : IRequest<FeeStructuresListResponse>;


