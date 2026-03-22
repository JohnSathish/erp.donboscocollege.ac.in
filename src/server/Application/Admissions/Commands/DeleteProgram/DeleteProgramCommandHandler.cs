using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.DeleteProgram;

public sealed class DeleteProgramCommandHandler : IRequestHandler<DeleteProgramCommand, bool>
{
    private readonly IProgramRepository _repository;

    public DeleteProgramCommandHandler(IProgramRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteProgramCommand request, CancellationToken cancellationToken)
    {
        var program = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (program == null)
        {
            return false;
        }

        await _repository.DeleteAsync(program, cancellationToken);
        return true;
    }
}









