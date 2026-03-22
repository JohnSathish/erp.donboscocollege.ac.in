using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public sealed class FeeComponentRepository(ApplicationDbContext context) : IFeeComponentRepository
{
    public async Task<FeeComponent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.FeeComponents
            .Include(x => x.FeeStructure)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<FeeComponent>> GetByFeeStructureIdAsync(Guid feeStructureId, CancellationToken cancellationToken = default)
    {
        return await context.FeeComponents
            .Where(x => x.FeeStructureId == feeStructureId)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<FeeComponent> AddAsync(FeeComponent component, CancellationToken cancellationToken = default)
    {
        await context.FeeComponents.AddAsync(component, cancellationToken);
        return component;
    }

    public async Task UpdateAsync(FeeComponent component, CancellationToken cancellationToken = default)
    {
        context.FeeComponents.Update(component);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(FeeComponent component, CancellationToken cancellationToken = default)
    {
        context.FeeComponents.Remove(component);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}









