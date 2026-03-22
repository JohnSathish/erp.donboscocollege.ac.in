using ERP.Application.Transport.Interfaces;
using MediatR;

namespace ERP.Application.Transport.Queries.GetVehicle;

public sealed class GetVehicleQueryHandler : IRequestHandler<GetVehicleQuery, GetVehicleResult?>
{
    private readonly IVehicleRepository _repository;

    public GetVehicleQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetVehicleResult?> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _repository.GetByIdAsync(request.VehicleId, cancellationToken);
        
        if (vehicle == null)
        {
            return null;
        }

        return new GetVehicleResult(
            vehicle.Id,
            vehicle.VehicleNumber,
            vehicle.VehicleType,
            vehicle.Make,
            vehicle.Model,
            vehicle.Year,
            vehicle.Color,
            vehicle.Capacity,
            vehicle.RegistrationNumber,
            vehicle.RegistrationExpiryDate,
            vehicle.InsuranceNumber,
            vehicle.InsuranceExpiryDate,
            vehicle.DriverName,
            vehicle.DriverContactNumber,
            vehicle.Route,
            vehicle.Status.ToString(),
            vehicle.Notes,
            vehicle.CreatedOnUtc,
            vehicle.CreatedBy,
            vehicle.UpdatedOnUtc,
            vehicle.UpdatedBy);
    }
}

