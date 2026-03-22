using ERP.Application.Transport.Interfaces;
using MediatR;

namespace ERP.Application.Transport.Queries.ListVehicles;

public sealed class ListVehiclesQueryHandler : IRequestHandler<ListVehiclesQuery, ListVehiclesResult>
{
    private readonly IVehicleRepository _vehicleRepository;

    public ListVehiclesQueryHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<ListVehiclesResult> Handle(ListVehiclesQuery request, CancellationToken cancellationToken)
    {
        var (vehicles, totalCount) = await _vehicleRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.VehicleType,
            request.Status,
            request.SearchTerm,
            cancellationToken);

        var vehicleDtos = vehicles.Select(v => new VehicleDto(
            v.Id,
            v.VehicleNumber,
            v.VehicleType,
            v.Make,
            v.Model,
            v.Year,
            v.Color,
            v.Capacity,
            v.RegistrationNumber,
            v.RegistrationExpiryDate,
            v.InsuranceNumber,
            v.InsuranceExpiryDate,
            v.DriverName,
            v.DriverContactNumber,
            v.Route,
            v.Status.ToString(),
            v.Notes,
            v.CreatedOnUtc,
            v.CreatedBy)).ToList();

        return new ListVehiclesResult(
            vehicleDtos,
            totalCount,
            request.Page,
            request.PageSize);
    }
}




