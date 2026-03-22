using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.ToggleFeeStructureStatus;

public sealed class ToggleFeeStructureStatusCommandHandler : IRequestHandler<ToggleFeeStructureStatusCommand, bool>
{
    private readonly IFeeStructureRepository _repository;

    public ToggleFeeStructureStatusCommandHandler(IFeeStructureRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ToggleFeeStructureStatusCommand request, CancellationToken cancellationToken)
    {
        var feeStructure = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (feeStructure == null)
        {
            return false;
        }

        feeStructure.ToggleStatus(request.UpdatedBy);
        await _repository.UpdateAsync(feeStructure, cancellationToken);
        return true;
    }
}


