using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Students;

public class StudentExitRepository : IStudentExitRepository
{
    private readonly ApplicationDbContext _context;

    public StudentExitRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentExit> AddAsync(StudentExit exit, CancellationToken cancellationToken = default)
    {
        var entry = await _context.StudentExits.AddAsync(exit, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public Task<StudentExit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.StudentExits
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task<StudentExit?> GetActiveExitByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return _context.StudentExits
            .AsNoTracking()
            .Where(e => e.StudentId == studentId && 
                       (e.Status == ExitStatus.Pending || e.Status == ExitStatus.Approved))
            .OrderByDescending(e => e.RequestedDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<StudentExit>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.StudentExits
            .AsNoTracking()
            .Where(e => e.StudentId == studentId)
            .OrderByDescending(e => e.RequestedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<StudentExit>> GetPendingExitsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StudentExits
            .AsNoTracking()
            .Where(e => e.Status == ExitStatus.Pending)
            .OrderBy(e => e.RequestedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(StudentExit exit, CancellationToken cancellationToken = default)
    {
        _context.StudentExits.Update(exit);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

