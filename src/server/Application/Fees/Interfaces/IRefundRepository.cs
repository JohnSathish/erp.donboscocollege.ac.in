using ERP.Domain.Fees.Entities;

namespace ERP.Application.Fees.Interfaces;

public interface IRefundRepository
{
    Task<Refund> AddAsync(Refund refund, CancellationToken cancellationToken = default);
    Task<Refund?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Refund?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Refund?> GetByRefundNumberAsync(string refundNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Refund>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Refund>> GetByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Refund>> GetPendingRefundsAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Refund refund, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

