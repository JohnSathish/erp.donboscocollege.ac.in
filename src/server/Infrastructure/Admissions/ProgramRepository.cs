using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public sealed class ProgramRepository(ApplicationDbContext context) : IProgramRepository
{
    public async Task<Program?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Programs
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Program?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await context.Programs
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Program>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default)
    {
        var query = context.Programs.AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyCollection<Program> Programs, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        bool? isActive = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Programs.AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLowerInvariant();
            query = query.Where(x =>
                x.Name.ToLower().Contains(term) ||
                x.Code.ToLower().Contains(term) ||
                (x.Description != null && x.Description.ToLower().Contains(term)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var programs = await query
            .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (programs, totalCount);
    }

    public async Task<Program> AddAsync(Program program, CancellationToken cancellationToken = default)
    {
        await context.Programs.AddAsync(program, cancellationToken);
        return program;
    }

    public async Task UpdateAsync(Program program, CancellationToken cancellationToken = default)
    {
        context.Programs.Update(program);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Program program, CancellationToken cancellationToken = default)
    {
        context.Programs.Remove(program);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}









