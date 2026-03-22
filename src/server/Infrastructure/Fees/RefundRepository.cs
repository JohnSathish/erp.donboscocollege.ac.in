using ERP.Application.Fees.Interfaces;
using ERP.Domain.Fees.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Fees;

public class RefundRepository : IRefundRepository
{
    private readonly ApplicationDbContext _context;

    public RefundRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Refund> AddAsync(Refund refund, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Refunds.AddAsync(refund, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public Task<Refund?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Refunds
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task<Refund?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Refunds
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task<Refund?> GetByRefundNumberAsync(string refundNumber, CancellationToken cancellationToken = default)
    {
        return _context.Refunds
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RefundNumber == refundNumber, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Refund>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.Refunds
            .AsNoTracking()
            .Where(r => r.StudentId == studentId)
            .OrderByDescending(r => r.RequestedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Refund>> GetByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default)
    {
        return await _context.Refunds
            .AsNoTracking()
            .Where(r => r.PaymentId == paymentId)
            .OrderByDescending(r => r.RequestedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Refund>> GetPendingRefundsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Refunds
            .AsNoTracking()
            .Where(r => r.Status == RefundStatus.Pending || r.Status == RefundStatus.Approved)
            .OrderBy(r => r.RequestedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Refund refund, CancellationToken cancellationToken = default)
    {
        _context.Refunds.Update(refund);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

