using ERP.Application.Admissions.Queries.ListPrograms;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListPrograms;

public sealed record ListProgramsQuery(
    int Page = 1,
    int PageSize = 50,
    bool? IsActive = null,
    string? SearchTerm = null) : IRequest<ProgramsListResponse>;









