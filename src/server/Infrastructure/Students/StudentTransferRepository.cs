using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Students;

public class StudentTransferRepository : IStudentTransferRepository
{
    private readonly ApplicationDbContext _context;

    public StudentTransferRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentTransfer> AddAsync(StudentTransfer transfer, CancellationToken cancellationToken = default)
    {
        var entry = await _context.StudentTransfers.AddAsync(transfer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public Task<StudentTransfer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.StudentTransfers
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<StudentTransfer>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.StudentTransfers
            .AsNoTracking()
            .Where(t => t.StudentId == studentId)
            .OrderByDescending(t => t.RequestedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<StudentTransfer>> GetPendingTransfersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StudentTransfers
            .AsNoTracking()
            .Where(t => t.Status == TransferStatus.Pending)
            .OrderBy(t => t.RequestedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(StudentTransfer transfer, CancellationToken cancellationToken = default)
    {
        _context.StudentTransfers.Update(transfer);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

