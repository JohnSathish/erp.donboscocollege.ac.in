using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.CreateFeeStructure;

public sealed class CreateFeeStructureCommandHandler : IRequestHandler<CreateFeeStructureCommand, Guid>
{
    private readonly IFeeStructureRepository _feeStructureRepository;
    private readonly IProgramRepository _programRepository;

    public CreateFeeStructureCommandHandler(
        IFeeStructureRepository feeStructureRepository,
        IProgramRepository programRepository)
    {
        _feeStructureRepository = feeStructureRepository;
        _programRepository = programRepository;
    }

    public async Task<Guid> Handle(CreateFeeStructureCommand request, CancellationToken cancellationToken)
    {
        if (request.ProgramId.HasValue)
        {
            var program = await _programRepository.GetByIdAsync(request.ProgramId.Value, cancellationToken);
            if (program == null)
            {
                throw new InvalidOperationException($"Program with ID '{request.ProgramId}' not found.");
            }
        }

        var feeStructure = new FeeStructure(
            request.Name,
            request.AcademicYear,
            request.EffectiveFromUtc,
            request.ProgramId,
            request.Description,
            request.EffectiveToUtc,
            request.CreatedBy);

        if (request.Components != null && request.Components.Any())
        {
            foreach (var componentRequest in request.Components)
            {
                var component = new FeeComponent(
                    feeStructure.Id,
                    componentRequest.Name,
                    componentRequest.Amount,
                    componentRequest.IsOptional,
                    componentRequest.InstallmentNumber,
                    componentRequest.DueDateUtc,
                    componentRequest.Description,
                    componentRequest.DisplayOrder,
                    request.CreatedBy);

                feeStructure.AddComponent(component);
            }
        }

        await _feeStructureRepository.AddAsync(feeStructure, cancellationToken);
        await _feeStructureRepository.SaveChangesAsync(cancellationToken);

        return feeStructure.Id;
    }
}









