using ERP.Domain.Fees.Entities;

namespace ERP.Application.Fees.Interfaces;

public interface IInvoiceRepository
{
    Task<Invoice> AddAsync(Invoice invoice, CancellationToken cancellationToken = default);
    Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Invoice?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Invoice>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Invoice>> GetOverdueInvoicesAsync(DateTime? asOfDate = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Invoice>> GetPendingInvoicesAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

