using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.DeleteFeeComponent;

public sealed class DeleteFeeComponentCommandHandler : IRequestHandler<DeleteFeeComponentCommand, bool>
{
    private readonly IFeeComponentRepository _repository;

    public DeleteFeeComponentCommandHandler(IFeeComponentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteFeeComponentCommand request, CancellationToken cancellationToken)
    {
        var component = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (component == null)
        {
            return false;
        }

        await _repository.DeleteAsync(component, cancellationToken);
        return true;
    }
}



