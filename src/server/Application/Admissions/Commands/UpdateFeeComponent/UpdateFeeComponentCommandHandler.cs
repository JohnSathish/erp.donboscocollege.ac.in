using ERP.Application.Admissions.Interfaces;
using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateFeeComponent;

public sealed class UpdateFeeComponentCommandHandler : IRequestHandler<UpdateFeeComponentCommand, bool>
{
    private readonly IFeeComponentRepository _repository;

    public UpdateFeeComponentCommandHandler(IFeeComponentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateFeeComponentCommand request, CancellationToken cancellationToken)
    {
        var component = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (component == null)
        {
            return false;
        }

        component.Update(
            request.Name,
            request.Amount,
            request.IsOptional,
            request.InstallmentNumber,
            request.DueDateUtc,
            request.Description,
            request.DisplayOrder,
            request.UpdatedBy);

        await _repository.UpdateAsync(component, cancellationToken);
        return true;
    }
}



