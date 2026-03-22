using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateFeeStructure;

public sealed class UpdateFeeStructureCommandHandler : IRequestHandler<UpdateFeeStructureCommand, bool>
{
    private readonly IFeeStructureRepository _repository;
    private readonly IProgramRepository _programRepository;

    public UpdateFeeStructureCommandHandler(
        IFeeStructureRepository repository,
        IProgramRepository programRepository)
    {
        _repository = repository;
        _programRepository = programRepository;
    }

    public async Task<bool> Handle(UpdateFeeStructureCommand request, CancellationToken cancellationToken)
    {
        var feeStructure = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (feeStructure == null)
        {
            return false;
        }

        if (request.ProgramId.HasValue)
        {
            var program = await _programRepository.GetByIdAsync(request.ProgramId.Value, cancellationToken);
            if (program == null)
            {
                throw new InvalidOperationException($"Program with ID '{request.ProgramId}' not found.");
            }
        }

        feeStructure.Update(
            request.Name,
            request.AcademicYear,
            request.EffectiveFromUtc,
            request.ProgramId,
            request.Description,
            request.EffectiveToUtc,
            request.UpdatedBy);

        await _repository.UpdateAsync(feeStructure, cancellationToken);
        return true;
    }
}









