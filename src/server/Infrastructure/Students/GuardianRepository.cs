using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Students;

public class GuardianRepository : IGuardianRepository
{
    private readonly ApplicationDbContext _context;

    public GuardianRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guardian> AddAsync(Guardian guardian, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Guardians.AddAsync(guardian, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<IReadOnlyCollection<Guardian>> AddRangeAsync(IReadOnlyCollection<Guardian> guardians, CancellationToken cancellationToken = default)
    {
        await _context.Guardians.AddRangeAsync(guardians, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return guardians;
    }

    public Task<Guardian?> GetByIdAsync(Guid guardianId, CancellationToken cancellationToken = default)
    {
        return _context.Guardians
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == guardianId, cancellationToken);
    }

    public Task<IReadOnlyCollection<Guardian>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return _context.Guardians
            .AsNoTracking()
            .Where(g => g.StudentId == studentId && g.IsActive)
            .OrderBy(g => g.IsPrimary ? 0 : 1)
            .ThenBy(g => g.Relationship)
            .ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyCollection<Guardian>)t.Result);
    }

    public async Task UpdateAsync(Guardian guardian, CancellationToken cancellationToken = default)
    {
        _context.Guardians.Update(guardian);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}

