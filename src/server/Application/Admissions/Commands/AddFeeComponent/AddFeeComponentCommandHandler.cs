using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.AddFeeComponent;

public sealed class AddFeeComponentCommandHandler : IRequestHandler<AddFeeComponentCommand, Guid>
{
    private readonly IFeeStructureRepository _feeStructureRepository;
    private readonly IFeeComponentRepository _feeComponentRepository;

    public AddFeeComponentCommandHandler(
        IFeeStructureRepository feeStructureRepository,
        IFeeComponentRepository feeComponentRepository)
    {
        _feeStructureRepository = feeStructureRepository;
        _feeComponentRepository = feeComponentRepository;
    }

    public async Task<Guid> Handle(AddFeeComponentCommand request, CancellationToken cancellationToken)
    {
        var feeStructure = await _feeStructureRepository.GetByIdAsync(request.FeeStructureId, cancellationToken);
        if (feeStructure == null)
        {
            throw new InvalidOperationException($"Fee structure with ID '{request.FeeStructureId}' not found.");
        }

        var component = new FeeComponent(
            request.FeeStructureId,
            request.Name,
            request.Amount,
            request.IsOptional,
            request.InstallmentNumber,
            request.DueDateUtc,
            request.Description,
            request.DisplayOrder,
            request.CreatedBy);

        feeStructure.AddComponent(component);
        await _feeComponentRepository.AddAsync(component, cancellationToken);
        await _feeStructureRepository.SaveChangesAsync(cancellationToken);

        return component.Id;
    }
}









