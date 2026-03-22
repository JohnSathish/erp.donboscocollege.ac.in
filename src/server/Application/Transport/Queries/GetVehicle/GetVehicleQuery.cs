using MediatR;

namespace ERP.Application.Transport.Queries.GetVehicle;

public sealed record GetVehicleQuery(Guid VehicleId) : IRequest<GetVehicleResult?>;

public sealed record GetVehicleResult(
    Guid Id,
    string VehicleNumber,
    string VehicleType,
    string Make,
    string Model,
    int? Year,
    string? Color,
    int Capacity,
    string? RegistrationNumber,
    DateTime? RegistrationExpiryDate,
    string? InsuranceNumber,
    DateTime? InsuranceExpiryDate,
    string? DriverName,
    string? DriverContactNumber,
    string? Route,
    string Status,
    string? Notes,
    DateTime CreatedOnUtc,
    string? CreatedBy,
    DateTime? UpdatedOnUtc,
    string? UpdatedBy);

