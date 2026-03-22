using ERP.Domain.Transport.Entities;

namespace ERP.Application.Transport.Interfaces;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Vehicle?> GetByVehicleNumberAsync(string vehicleNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Vehicle> Vehicles, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? vehicleType = null,
        VehicleStatus? status = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    Task<Vehicle> AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task<bool> VehicleNumberExistsAsync(string vehicleNumber, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}




