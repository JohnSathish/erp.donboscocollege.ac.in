using ERP.Domain.Fees.Entities;

namespace ERP.Application.Fees.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Payment?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Payment?> GetByPaymentNumberAsync(string paymentNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Payment>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Payment>> GetByInvoiceIdAsync(Guid invoiceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Payment>> GetPendingPaymentsAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

