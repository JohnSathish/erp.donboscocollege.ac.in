using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Queries.ListFeeStructures;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListFeeStructures;

public sealed class ListFeeStructuresQueryHandler : IRequestHandler<ListFeeStructuresQuery, FeeStructuresListResponse>
{
    private readonly IFeeStructureRepository _repository;

    public ListFeeStructuresQueryHandler(IFeeStructureRepository repository)
    {
        _repository = repository;
    }

    public async Task<FeeStructuresListResponse> Handle(ListFeeStructuresQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var (feeStructures, totalCount) = await _repository.GetPagedAsync(
            page,
            pageSize,
            request.IsActive,
            request.ProgramId,
            request.AcademicYear,
            request.SearchTerm,
            cancellationToken);

        var feeStructureDtos = feeStructures.Select(fs => new FeeStructureDto(
            fs.Id,
            fs.Name,
            fs.Description,
            fs.ProgramId,
            fs.Program?.Name,
            fs.AcademicYear,
            fs.IsActive,
            fs.EffectiveFromUtc,
            fs.EffectiveToUtc,
            fs.GetTotalAmount(),
            fs.Components.Select(c => new FeeComponentDto(
                c.Id,
                c.Name,
                c.Description,
                c.Amount,
                c.IsOptional,
                c.InstallmentNumber,
                c.DueDateUtc,
                c.DisplayOrder,
                c.CreatedOnUtc,
                c.CreatedBy,
                c.UpdatedOnUtc,
                c.UpdatedBy)).ToList(),
            fs.CreatedOnUtc,
            fs.CreatedBy,
            fs.UpdatedOnUtc,
            fs.UpdatedBy)).ToList();

        return new FeeStructuresListResponse(feeStructureDtos, totalCount, page, pageSize);
    }
}


