using ERP.Application.Fees.Interfaces;
using ERP.Domain.Fees.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Fees;

public class ReceiptRepository : IReceiptRepository
{
    private readonly ApplicationDbContext _context;

    public ReceiptRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Receipt> AddAsync(Receipt receipt, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Receipts.AddAsync(receipt, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public Task<Receipt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Receipts
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task<Receipt?> GetByReceiptNumberAsync(string receiptNumber, CancellationToken cancellationToken = default)
    {
        return _context.Receipts
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ReceiptNumber == receiptNumber, cancellationToken);
    }

    public Task<Receipt?> GetByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default)
    {
        return _context.Receipts
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.PaymentId == paymentId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Receipt>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.Receipts
            .AsNoTracking()
            .Where(r => r.StudentId == studentId)
            .OrderByDescending(r => r.ReceiptDate)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Receipt receipt, CancellationToken cancellationToken = default)
    {
        _context.Receipts.Update(receipt);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

