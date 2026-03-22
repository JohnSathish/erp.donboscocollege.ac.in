using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Queries.ListPrograms;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListPrograms;

public sealed class ListProgramsQueryHandler : IRequestHandler<ListProgramsQuery, ProgramsListResponse>
{
    private readonly IProgramRepository _repository;

    public ListProgramsQueryHandler(IProgramRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProgramsListResponse> Handle(ListProgramsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var (programs, totalCount) = await _repository.GetPagedAsync(
            page,
            pageSize,
            request.IsActive,
            request.SearchTerm,
            cancellationToken);

        var programDtos = programs.Select(p => new ProgramDto(
            p.Id,
            p.Code,
            p.Name,
            p.Description,
            p.Level,
            p.DurationYears,
            p.TotalCredits,
            p.IsActive,
            p.CreatedOnUtc,
            p.CreatedBy,
            p.UpdatedOnUtc,
            p.UpdatedBy)).ToList();

        return new ProgramsListResponse(programDtos, totalCount, page, pageSize);
    }
}









