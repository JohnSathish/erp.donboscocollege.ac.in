using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Queries.GetProgramById;
using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetProgramById;

public sealed class GetProgramByIdQueryHandler : IRequestHandler<GetProgramByIdQuery, ProgramDto?>
{
    private readonly IProgramRepository _repository;

    public GetProgramByIdQueryHandler(IProgramRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProgramDto?> Handle(GetProgramByIdQuery request, CancellationToken cancellationToken)
    {
        var program = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (program == null)
        {
            return null;
        }

        return new ProgramDto(
            program.Id,
            program.Code,
            program.Name,
            program.Description,
            program.Level,
            program.DurationYears,
            program.TotalCredits,
            program.IsActive,
            program.CreatedOnUtc,
            program.CreatedBy,
            program.UpdatedOnUtc,
            program.UpdatedBy);
    }
}









