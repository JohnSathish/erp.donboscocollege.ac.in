using ERP.Domain.Fees.Entities;

namespace ERP.Application.Fees.Interfaces;

public interface IRefundService
{
    Task<Refund> CreateRefundRequestAsync(
        Guid paymentId,
        decimal amount,
        RefundReason reason,
        string? reasonDetails = null,
        string? remarks = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default);

    Task<bool> ApproveRefundAsync(
        Guid refundId,
        string processedBy,
        string? remarks = null,
        CancellationToken cancellationToken = default);

    Task<bool> ProcessRefundAsync(
        Guid refundId,
        string processedBy,
        string? remarks = null,
        CancellationToken cancellationToken = default);

    Task<bool> RejectRefundAsync(
        Guid refundId,
        string rejectedBy,
        string rejectionReason,
        CancellationToken cancellationToken = default);

    Task<string> GenerateRefundNumberAsync(
        string academicYear,
        CancellationToken cancellationToken = default);
}

