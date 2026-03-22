using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.CreateProgram;

public sealed class CreateProgramCommandHandler : IRequestHandler<CreateProgramCommand, Guid>
{
    private readonly IProgramRepository _repository;

    public CreateProgramCommandHandler(IProgramRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateProgramCommand request, CancellationToken cancellationToken)
    {
        var existingProgram = await _repository.GetByCodeAsync(request.Code, cancellationToken);
        if (existingProgram != null)
        {
            throw new InvalidOperationException($"Program with code '{request.Code}' already exists.");
        }

        var program = new Program(
            request.Code,
            request.Name,
            request.Level,
            request.DurationYears,
            request.TotalCredits,
            request.Description,
            request.CreatedBy);

        await _repository.AddAsync(program, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return program.Id;
    }
}









