using MediatR;

namespace ERP.Application.Transport.Commands.CreateVehicle;

public sealed record CreateVehicleCommand(
    string VehicleNumber,
    string VehicleType,
    string Make,
    string Model,
    int Capacity,
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
    string? CreatedBy = null) : IRequest<Guid>;




