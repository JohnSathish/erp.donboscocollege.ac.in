using ERP.Application.Transport.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Transport.Commands.UpdateVehicle;

public sealed class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, Unit>
{
    private readonly IVehicleRepository _repository;
    private readonly ILogger<UpdateVehicleCommandHandler> _logger;

    public UpdateVehicleCommandHandler(
        IVehicleRepository repository,
        ILogger<UpdateVehicleCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _repository.GetByIdAsync(request.VehicleId, cancellationToken);
        
        if (vehicle == null)
        {
            throw new InvalidOperationException($"Vehicle with ID '{request.VehicleId}' not found.");
        }

        vehicle.UpdateDetails(
            request.VehicleType ?? vehicle.VehicleType,
            request.Make ?? vehicle.Make,
            request.Model ?? vehicle.Model,
            request.Capacity ?? vehicle.Capacity,
            request.Color ?? vehicle.Color,
            request.Year ?? vehicle.Year,
            request.RegistrationNumber ?? vehicle.RegistrationNumber,
            request.RegistrationExpiryDate ?? vehicle.RegistrationExpiryDate,
            request.InsuranceNumber ?? vehicle.InsuranceNumber,
            request.InsuranceExpiryDate ?? vehicle.InsuranceExpiryDate,
            request.DriverName ?? vehicle.DriverName,
            request.DriverContactNumber ?? vehicle.DriverContactNumber,
            request.Route ?? vehicle.Route,
            request.Notes ?? vehicle.Notes,
            request.UpdatedBy);

        await _repository.UpdateAsync(vehicle, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Updated vehicle {VehicleId} ({VehicleNumber}) by {UpdatedBy}",
            vehicle.Id,
            vehicle.VehicleNumber,
            request.UpdatedBy ?? "System");

        return Unit.Value;
    }
}

