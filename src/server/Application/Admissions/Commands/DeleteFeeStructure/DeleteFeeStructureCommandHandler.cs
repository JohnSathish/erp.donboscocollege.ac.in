using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.DeleteFeeStructure;

public sealed class DeleteFeeStructureCommandHandler : IRequestHandler<DeleteFeeStructureCommand, bool>
{
    private readonly IFeeStructureRepository _repository;

    public DeleteFeeStructureCommandHandler(IFeeStructureRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteFeeStructureCommand request, CancellationToken cancellationToken)
    {
        var feeStructure = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (feeStructure == null)
        {
            return false;
        }

        await _repository.DeleteAsync(feeStructure, cancellationToken);
        return true;
    }
}



