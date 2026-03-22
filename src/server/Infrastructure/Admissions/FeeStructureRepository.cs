using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public sealed class FeeStructureRepository(ApplicationDbContext context) : IFeeStructureRepository
{
    public async Task<FeeStructure?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.FeeStructures
            .Include(x => x.Program)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<FeeStructure?> GetByIdWithComponentsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.FeeStructures
            .Include(x => x.Program)
            .Include(x => x.Components.OrderBy(c => c.DisplayOrder))
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<FeeStructure>> GetByProgramIdAsync(Guid programId, CancellationToken cancellationToken = default)
    {
        return await context.FeeStructures
            .Include(x => x.Program)
            .Include(x => x.Components)
            .Where(x => x.ProgramId == programId)
            .OrderByDescending(x => x.AcademicYear)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<FeeStructure>> GetByAcademicYearAsync(string academicYear, CancellationToken cancellationToken = default)
    {
        return await context.FeeStructures
            .Include(x => x.Program)
            .Include(x => x.Components)
            .Where(x => x.AcademicYear == academicYear)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<FeeStructure>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default)
    {
        var query = context.FeeStructures
            .Include(x => x.Program)
            .Include(x => x.Components)
            .AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        return await query
            .OrderByDescending(x => x.AcademicYear)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyCollection<FeeStructure> FeeStructures, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        bool? isActive = null,
        Guid? programId = null,
        string? academicYear = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.FeeStructures
            .Include(x => x.Program)
            .Include(x => x.Components)
            .AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        if (programId.HasValue)
        {
            query = query.Where(x => x.ProgramId == programId.Value);
        }

        if (!string.IsNullOrWhiteSpace(academicYear))
        {
            query = query.Where(x => x.AcademicYear == academicYear);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLowerInvariant();
            query = query.Where(x =>
                x.Name.ToLower().Contains(term) ||
                x.AcademicYear.ToLower().Contains(term) ||
                (x.Description != null && x.Description.ToLower().Contains(term)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var feeStructures = await query
            .OrderByDescending(x => x.AcademicYear)
            .ThenBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (feeStructures, totalCount);
    }

    public async Task<FeeStructure> AddAsync(FeeStructure feeStructure, CancellationToken cancellationToken = default)
    {
        await context.FeeStructures.AddAsync(feeStructure, cancellationToken);
        return feeStructure;
    }

    public async Task UpdateAsync(FeeStructure feeStructure, CancellationToken cancellationToken = default)
    {
        context.FeeStructures.Update(feeStructure);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(FeeStructure feeStructure, CancellationToken cancellationToken = default)
    {
        context.FeeStructures.Remove(feeStructure);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}









