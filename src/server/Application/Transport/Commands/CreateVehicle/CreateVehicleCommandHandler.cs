using ERP.Application.Transport.Interfaces;
using ERP.Domain.Transport.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Transport.Commands.CreateVehicle;

public sealed class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, Guid>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ILogger<CreateVehicleCommandHandler> _logger;

    public CreateVehicleCommandHandler(
        IVehicleRepository vehicleRepository,
        ILogger<CreateVehicleCommandHandler> logger)
    {
        _vehicleRepository = vehicleRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        // Check if vehicle number already exists
        var exists = await _vehicleRepository.VehicleNumberExistsAsync(request.VehicleNumber, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException(
                $"Vehicle with number '{request.VehicleNumber}' already exists.");
        }

        var vehicle = new Vehicle(
            request.VehicleNumber,
            request.VehicleType,
            request.Make,
            request.Model,
            request.Capacity,
            request.Color,
            request.Year,
            request.RegistrationNumber,
            request.RegistrationExpiryDate,
            request.InsuranceNumber,
            request.InsuranceExpiryDate,
            request.DriverName,
            request.DriverContactNumber,
            request.Route,
            request.Notes,
            request.CreatedBy);

        await _vehicleRepository.AddAsync(vehicle, cancellationToken);

        _logger.LogInformation(
            "Created vehicle {VehicleNumber} ({Make} {Model})",
            vehicle.VehicleNumber,
            vehicle.Make,
            vehicle.Model);

        return vehicle.Id;
    }
}




