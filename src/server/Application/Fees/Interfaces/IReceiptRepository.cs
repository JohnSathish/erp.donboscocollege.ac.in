using ERP.Domain.Fees.Entities;

namespace ERP.Application.Fees.Interfaces;

public interface IReceiptRepository
{
    Task<Receipt> AddAsync(Receipt receipt, CancellationToken cancellationToken = default);
    Task<Receipt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Receipt?> GetByReceiptNumberAsync(string receiptNumber, CancellationToken cancellationToken = default);
    Task<Receipt?> GetByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Receipt>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Receipt receipt, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

