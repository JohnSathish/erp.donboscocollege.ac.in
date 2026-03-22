using MediatR;
using ERP.Domain.Transport.Entities;

namespace ERP.Application.Transport.Queries.ListVehicles;

public sealed record ListVehiclesQuery(
    int Page = 1,
    int PageSize = 50,
    string? VehicleType = null,
    VehicleStatus? Status = null,
    string? SearchTerm = null) : IRequest<ListVehiclesResult>;

public sealed record ListVehiclesResult(
    IReadOnlyCollection<VehicleDto> Vehicles,
    int TotalCount,
    int Page,
    int PageSize);

public sealed record VehicleDto(
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
    string? CreatedBy);




