using ERP.Application.Fees.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Fees.Entities;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Fees;

public class RefundService : IRefundService
{
    private readonly IRefundRepository _refundRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<RefundService> _logger;

    public RefundService(
        IRefundRepository refundRepository,
        IPaymentRepository paymentRepository,
        IInvoiceRepository invoiceRepository,
        IStudentRepository studentRepository,
        ILogger<RefundService> logger)
    {
        _refundRepository = refundRepository;
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<Refund> CreateRefundRequestAsync(
        Guid paymentId,
        decimal amount,
        RefundReason reason,
        string? reasonDetails = null,
        string? remarks = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);
        if (payment == null)
            throw new InvalidOperationException($"Payment with ID {paymentId} not found.");

        if (payment.Status != PaymentStatus.Completed)
            throw new InvalidOperationException("Refunds can only be requested for completed payments.");

        if (amount <= 0)
            throw new ArgumentException("Refund amount must be greater than zero.", nameof(amount));

        if (amount > payment.Amount)
            throw new ArgumentException($"Refund amount ({amount}) cannot exceed payment amount ({payment.Amount}).", nameof(amount));

        // Check if there's already a pending or approved refund for this payment
        var existingRefunds = await _refundRepository.GetByPaymentIdAsync(paymentId, cancellationToken);
        var totalRefunded = existingRefunds
            .Where(r => r.Status == RefundStatus.Processed)
            .Sum(r => r.Amount);

        if (amount + totalRefunded > payment.Amount)
            throw new InvalidOperationException($"Total refund amount ({amount + totalRefunded}) cannot exceed payment amount ({payment.Amount}).");

        var student = await _studentRepository.GetByIdAsync(payment.StudentId, cancellationToken);
        if (student == null)
            throw new InvalidOperationException($"Student with ID {payment.StudentId} not found.");

        var refundNumber = await GenerateRefundNumberAsync(DateTime.UtcNow.Year.ToString(), cancellationToken);

        var refund = new Refund(
            payment.StudentId,
            paymentId,
            refundNumber,
            amount,
            reason,
            payment.InvoiceId,
            reasonDetails,
            remarks,
            createdBy);

        var createdRefund = await _refundRepository.AddAsync(refund, cancellationToken);

        _logger.LogInformation("Refund request {RefundNumber} created for payment {PaymentNumber} (Amount: {Amount}, Reason: {Reason}).",
            refundNumber, payment.PaymentNumber, amount, reason);

        return createdRefund;
    }

    public async Task<bool> ApproveRefundAsync(
        Guid refundId,
        string processedBy,
        string? remarks = null,
        CancellationToken cancellationToken = default)
    {
        var refund = await _refundRepository.GetByIdAsync(refundId, cancellationToken);
        if (refund == null)
        {
            _logger.LogWarning("Refund with ID {RefundId} not found for approval.", refundId);
            return false;
        }

        var refundForUpdate = await _refundRepository.GetByIdForUpdateAsync(refundId, cancellationToken);
        if (refundForUpdate == null)
        {
            _logger.LogWarning("Refund with ID {RefundId} not found for approval.", refundId);
            return false;
        }

        refundForUpdate.Approve(processedBy, remarks);
        await _refundRepository.UpdateAsync(refundForUpdate, cancellationToken);

        _logger.LogInformation("Refund {RefundNumber} approved by {ProcessedBy}.", refundForUpdate.RefundNumber, processedBy);
        return true;
    }

    public async Task<bool> ProcessRefundAsync(
        Guid refundId,
        string processedBy,
        string? remarks = null,
        CancellationToken cancellationToken = default)
    {
        var refund = await _refundRepository.GetByIdAsync(refundId, cancellationToken);
        if (refund == null)
        {
            _logger.LogWarning("Refund with ID {RefundId} not found for processing.", refundId);
            return false;
        }

        var refundForUpdate = await _refundRepository.GetByIdForUpdateAsync(refundId, cancellationToken);
        if (refundForUpdate == null)
        {
            _logger.LogWarning("Refund with ID {RefundId} not found for processing.", refundId);
            return false;
        }

        refundForUpdate.Process(processedBy, remarks);
        await _refundRepository.UpdateAsync(refundForUpdate, cancellationToken);

        // Update payment status
        var payment = await _paymentRepository.GetByIdForUpdateAsync(refundForUpdate.PaymentId, cancellationToken);
        if (payment != null)
        {
            payment.MarkAsRefunded(processedBy);
            await _paymentRepository.UpdateAsync(payment, cancellationToken);
        }

        // Update invoice if applicable
        if (refundForUpdate.InvoiceId.HasValue)
        {
            var invoice = await _invoiceRepository.GetByIdForUpdateAsync(refundForUpdate.InvoiceId.Value, cancellationToken);
            if (invoice != null)
            {
                invoice.RecordRefund(refundForUpdate.Amount, processedBy);
                await _invoiceRepository.UpdateAsync(invoice, cancellationToken);
            }
        }

        _logger.LogInformation("Refund {RefundNumber} processed by {ProcessedBy}.", refundForUpdate.RefundNumber, processedBy);
        return true;
    }

    public async Task<bool> RejectRefundAsync(
        Guid refundId,
        string rejectedBy,
        string rejectionReason,
        CancellationToken cancellationToken = default)
    {
        var refund = await _refundRepository.GetByIdAsync(refundId, cancellationToken);
        if (refund == null)
        {
            _logger.LogWarning("Refund with ID {RefundId} not found for rejection.", refundId);
            return false;
        }

        var refundForUpdate = await _refundRepository.GetByIdForUpdateAsync(refundId, cancellationToken);
        if (refundForUpdate == null)
        {
            _logger.LogWarning("Refund with ID {RefundId} not found for rejection.", refundId);
            return false;
        }

        refundForUpdate.Reject(rejectedBy, rejectionReason);
        await _refundRepository.UpdateAsync(refundForUpdate, cancellationToken);

        _logger.LogInformation("Refund {RefundNumber} rejected by {RejectedBy}. Reason: {Reason}.",
            refundForUpdate.RefundNumber, rejectedBy, rejectionReason);
        return true;
    }

    public async Task<string> GenerateRefundNumberAsync(
        string academicYear,
        CancellationToken cancellationToken = default)
    {
        var yearPrefix = academicYear.Replace("-", "").Substring(0, 4);
        var prefix = $"REF-{yearPrefix}-";
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        return $"{prefix}{timestamp.Substring(timestamp.Length - 6)}";
    }
}

