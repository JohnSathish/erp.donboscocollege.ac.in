using ERP.Application.Fees.Interfaces;
using ERP.Domain.Fees.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Fees;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly ApplicationDbContext _context;

    public InvoiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice> AddAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Invoices.AddAsync(invoice, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Invoices
            .Include(i => i.Lines)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public Task<Invoice?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Invoices
            .Include(i => i.Lines)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default)
    {
        return _context.Invoices
            .Include(i => i.Lines)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Invoice>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.Invoices
            .Include(i => i.Lines)
            .AsNoTracking()
            .Where(i => i.StudentId == studentId)
            .OrderByDescending(i => i.IssueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Invoice>> GetOverdueInvoicesAsync(DateTime? asOfDate = null, CancellationToken cancellationToken = default)
    {
        var date = asOfDate ?? DateTime.UtcNow;
        return await _context.Invoices
            .Include(i => i.Lines)
            .AsNoTracking()
            .Where(i => i.DueDate < date && 
                       i.Status != InvoiceStatus.Paid && 
                       i.Status != InvoiceStatus.Cancelled &&
                       i.BalanceAmount > 0)
            .OrderBy(i => i.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Invoice>> GetPendingInvoicesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Invoices
            .Include(i => i.Lines)
            .AsNoTracking()
            .Where(i => i.Status == InvoiceStatus.Issued || i.Status == InvoiceStatus.PartiallyPaid)
            .OrderBy(i => i.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

