using ERP.Application.Fees.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Fees.Entities;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Fees;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IReceiptRepository _receiptRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IInvoiceRepository invoiceRepository,
        IReceiptRepository receiptRepository,
        IStudentRepository studentRepository,
        ILogger<PaymentService> logger)
    {
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
        _receiptRepository = receiptRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(
        Guid invoiceId,
        decimal amount,
        PaymentMethod paymentMethod,
        DateTime paymentDate,
        string? paymentGateway = null,
        string? transactionId = null,
        string? referenceNumber = null,
        string? remarks = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);
        if (invoice == null)
            return new PaymentResult(null!, null, false, $"Invoice with ID {invoiceId} not found.");

        if (invoice.Status == InvoiceStatus.Cancelled)
            return new PaymentResult(null!, null, false, "Cannot process payment for a cancelled invoice.");

        if (amount <= 0)
            return new PaymentResult(null!, null, false, "Payment amount must be greater than zero.");

        if (amount > invoice.BalanceAmount)
            return new PaymentResult(null!, null, false, $"Payment amount ({amount}) cannot exceed invoice balance ({invoice.BalanceAmount}).");

        var student = await _studentRepository.GetByIdAsync(invoice.StudentId, cancellationToken);
        if (student == null)
            return new PaymentResult(null!, null, false, $"Student with ID {invoice.StudentId} not found.");

        var paymentNumber = await GeneratePaymentNumberAsync(invoice.AcademicYear, cancellationToken);

        var payment = new Payment(
            invoice.StudentId,
            invoiceId,
            paymentNumber,
            amount,
            paymentMethod,
            paymentDate,
            paymentGateway,
            transactionId,
            referenceNumber,
            remarks,
            createdBy);

        // Mark payment as completed if transaction ID is provided (online payment)
        if (!string.IsNullOrWhiteSpace(transactionId))
        {
            payment.MarkAsCompleted(transactionId, createdBy);
        }

        var createdPayment = await _paymentRepository.AddAsync(payment, cancellationToken);

        // Update invoice with payment (need tracked entity)
        var invoiceForUpdate = await _invoiceRepository.GetByIdForUpdateAsync(invoiceId, cancellationToken);
        if (invoiceForUpdate != null)
        {
            invoiceForUpdate.RecordPayment(amount, createdBy);
            await _invoiceRepository.UpdateAsync(invoiceForUpdate, cancellationToken);
        }

        // Generate receipt if payment is completed
        Receipt? receipt = null;
        if (payment.Status == PaymentStatus.Completed)
        {
            receipt = await GenerateReceiptAsync(createdPayment.Id, createdBy, cancellationToken);
        }

        _logger.LogInformation("Payment {PaymentNumber} of {Amount} processed for invoice {InvoiceNumber} (Student: {StudentNumber}).",
            paymentNumber, amount, invoice.InvoiceNumber, student.StudentNumber);

        return new PaymentResult(createdPayment, receipt, true);
    }

    public async Task<Receipt> GenerateReceiptAsync(
        Guid paymentId,
        string? createdBy = null,
        CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);
        if (payment == null)
            throw new InvalidOperationException($"Payment with ID {paymentId} not found.");

        if (payment.Status != PaymentStatus.Completed)
            throw new InvalidOperationException("Receipt can only be generated for completed payments.");

        // Check if receipt already exists
        var existingReceipt = await _receiptRepository.GetByPaymentIdAsync(paymentId, cancellationToken);
        if (existingReceipt != null)
            return existingReceipt;

        var receiptNumber = await GenerateReceiptNumberAsync(payment.PaymentDate.Year.ToString(), cancellationToken);

        var receipt = new Receipt(
            payment.StudentId,
            paymentId,
            receiptNumber,
            payment.Amount,
            payment.PaymentDate,
            payment.Remarks,
            createdBy);

        var createdReceipt = await _receiptRepository.AddAsync(receipt, cancellationToken);

        // Link receipt to payment (need tracked entity)
        var paymentForUpdate = await _paymentRepository.GetByIdForUpdateAsync(paymentId, cancellationToken);
        if (paymentForUpdate != null)
        {
            paymentForUpdate.LinkReceipt(createdReceipt.Id, createdBy);
            await _paymentRepository.UpdateAsync(paymentForUpdate, cancellationToken);
        }

        _logger.LogInformation("Receipt {ReceiptNumber} generated for payment {PaymentNumber}.",
            receiptNumber, payment.PaymentNumber);

        return createdReceipt;
    }

    public async Task<string> GeneratePaymentNumberAsync(
        string academicYear,
        CancellationToken cancellationToken = default)
    {
        var yearPrefix = academicYear.Replace("-", "").Substring(0, 4);
        var prefix = $"PAY-{yearPrefix}-";
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        return $"{prefix}{timestamp.Substring(timestamp.Length - 6)}";
    }

    public async Task<string> GenerateReceiptNumberAsync(
        string academicYear,
        CancellationToken cancellationToken = default)
    {
        var yearPrefix = academicYear.Replace("-", "").Substring(0, 4);
        var prefix = $"RCP-{yearPrefix}-";
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        return $"{prefix}{timestamp.Substring(timestamp.Length - 6)}";
    }
}

