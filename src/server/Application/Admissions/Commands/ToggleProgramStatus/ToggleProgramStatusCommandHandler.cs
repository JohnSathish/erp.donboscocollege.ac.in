using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.ToggleProgramStatus;

public sealed class ToggleProgramStatusCommandHandler : IRequestHandler<ToggleProgramStatusCommand, bool>
{
    private readonly IProgramRepository _repository;

    public ToggleProgramStatusCommandHandler(IProgramRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ToggleProgramStatusCommand request, CancellationToken cancellationToken)
    {
        var program = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (program == null)
        {
            return false;
        }

        program.ToggleStatus(request.UpdatedBy);
        await _repository.UpdateAsync(program, cancellationToken);
        return true;
    }
}









