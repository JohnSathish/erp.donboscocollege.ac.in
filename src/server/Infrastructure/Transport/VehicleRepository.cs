using ERP.Application.Transport.Interfaces;
using ERP.Domain.Transport.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Transport;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public Task<Vehicle?> GetByVehicleNumberAsync(string vehicleNumber, CancellationToken cancellationToken = default)
    {
        return _context.Vehicles
            .FirstOrDefaultAsync(v => v.VehicleNumber == vehicleNumber, cancellationToken);
    }

    public Task<IReadOnlyCollection<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<Vehicle>>(
            _context.Vehicles
                .AsNoTracking()
                .ToList());
    }

    public async Task<(IReadOnlyCollection<Vehicle> Vehicles, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? vehicleType = null,
        VehicleStatus? status = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Vehicles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(vehicleType))
        {
            query = query.Where(v => v.VehicleType == vehicleType);
        }

        if (status.HasValue)
        {
            query = query.Where(v => v.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.Trim();
            query = query.Where(v =>
                v.VehicleNumber.Contains(search) ||
                v.Make.Contains(search) ||
                v.Model.Contains(search) ||
                (v.RegistrationNumber != null && v.RegistrationNumber.Contains(search)) ||
                (v.DriverName != null && v.DriverName.Contains(search)) ||
                (v.Route != null && v.Route.Contains(search)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var vehicles = await query
            .OrderBy(v => v.VehicleNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (vehicles, totalCount);
    }

    public async Task<Vehicle> AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        await _context.Vehicles.AddAsync(vehicle, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return vehicle;
    }

    public async Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> VehicleNumberExistsAsync(string vehicleNumber, CancellationToken cancellationToken = default)
    {
        return _context.Vehicles
            .AnyAsync(v => v.VehicleNumber == vehicleNumber, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}




