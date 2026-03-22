using MediatR;

namespace ERP.Application.Transport.Commands.UpdateVehicle;

public sealed record UpdateVehicleCommand(
    Guid VehicleId,
    string? VehicleType = null,
    string? Make = null,
    string? Model = null,
    int? Capacity = null,
    string? Color = null,
    int? Year = null,
    string? RegistrationNumber = null,
    DateTime? RegistrationExpiryDate = null,
    string? InsuranceNumber = null,
    DateTime? InsuranceExpiryDate = null,
    string? DriverName = null,
    string? DriverContactNumber = null,
    string? Route = null,
    string? Notes = null,
    string? UpdatedBy = null) : IRequest;

