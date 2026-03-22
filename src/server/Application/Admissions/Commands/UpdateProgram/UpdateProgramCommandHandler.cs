using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateProgram;

public sealed class UpdateProgramCommandHandler : IRequestHandler<UpdateProgramCommand, bool>
{
    private readonly IProgramRepository _repository;

    public UpdateProgramCommandHandler(IProgramRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateProgramCommand request, CancellationToken cancellationToken)
    {
        var program = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (program == null)
        {
            return false;
        }

        program.Update(
            request.Name,
            request.Level,
            request.DurationYears,
            request.TotalCredits,
            request.Description,
            request.UpdatedBy);

        await _repository.UpdateAsync(program, cancellationToken);
        return true;
    }
}









