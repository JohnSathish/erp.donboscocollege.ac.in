using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Queries.GetFeeStructureById;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetFeeStructureById;

public sealed class GetFeeStructureByIdQueryHandler : IRequestHandler<GetFeeStructureByIdQuery, FeeStructureDto?>
{
    private readonly IFeeStructureRepository _repository;

    public GetFeeStructureByIdQueryHandler(IFeeStructureRepository repository)
    {
        _repository = repository;
    }

    public async Task<FeeStructureDto?> Handle(GetFeeStructureByIdQuery request, CancellationToken cancellationToken)
    {
        var feeStructure = await _repository.GetByIdWithComponentsAsync(request.Id, cancellationToken);
        if (feeStructure == null)
        {
            return null;
        }

        return new FeeStructureDto(
            feeStructure.Id,
            feeStructure.Name,
            feeStructure.Description,
            feeStructure.ProgramId,
            feeStructure.Program?.Name,
            feeStructure.AcademicYear,
            feeStructure.IsActive,
            feeStructure.EffectiveFromUtc,
            feeStructure.EffectiveToUtc,
            feeStructure.GetTotalAmount(),
            feeStructure.Components.Select(c => new FeeComponentDto(
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
            feeStructure.CreatedOnUtc,
            feeStructure.CreatedBy,
            feeStructure.UpdatedOnUtc,
            feeStructure.UpdatedBy);
    }
}


