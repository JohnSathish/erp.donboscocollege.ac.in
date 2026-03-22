using ERP.Application.Fees.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Fees.Entities;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Fees;

public class FeeLedgerService : IFeeLedgerService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<FeeLedgerService> _logger;

    public FeeLedgerService(
        IInvoiceRepository invoiceRepository,
        IPaymentRepository paymentRepository,
        IStudentRepository studentRepository,
        ILogger<FeeLedgerService> logger)
    {
        _invoiceRepository = invoiceRepository;
        _paymentRepository = paymentRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<FeeLedgerSummaryDto> GetStudentFeeLedgerAsync(
        Guid studentId,
        string? academicYear = null,
        CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student == null)
            throw new InvalidOperationException($"Student with ID {studentId} not found.");

        var invoices = await _invoiceRepository.GetByStudentIdAsync(studentId, cancellationToken);
        var payments = await _paymentRepository.GetByStudentIdAsync(studentId, cancellationToken);

        if (!string.IsNullOrWhiteSpace(academicYear))
        {
            invoices = invoices.Where(i => i.AcademicYear == academicYear).ToList();
            payments = payments.Where(p => p.PaymentDate.Year.ToString() == academicYear.Substring(0, 4)).ToList();
        }

        var totalInvoiced = invoices.Sum(i => i.TotalAmount);
        var totalPaid = payments.Where(p => p.Status == PaymentStatus.Completed).Sum(p => p.Amount);
        var totalRefunded = payments.Where(p => p.Status == PaymentStatus.Refunded).Sum(p => p.Amount);
        var outstandingBalance = invoices
            .Where(i => i.Status != InvoiceStatus.Cancelled && i.Status != InvoiceStatus.Paid)
            .Sum(i => i.BalanceAmount);

        var invoiceSummaries = invoices.Select(i => new InvoiceSummaryDto(
            i.Id,
            i.InvoiceNumber,
            i.IssueDate,
            i.DueDate,
            i.TotalAmount,
            i.PaidAmount,
            i.BalanceAmount,
            i.Status)).ToList();

        var paymentSummaries = payments.Select(p => new PaymentSummaryDto(
            p.Id,
            p.PaymentNumber,
            p.PaymentDate,
            p.Amount,
            p.PaymentMethod,
            p.Status)).ToList();

        return new FeeLedgerSummaryDto(
            studentId,
            student.StudentNumber,
            student.FullName,
            totalInvoiced,
            totalPaid,
            totalRefunded,
            outstandingBalance,
            invoiceSummaries,
            paymentSummaries);
    }

    public async Task<AgingReportDto> GetAgingReportAsync(
        DateTime? asOfDate = null,
        Guid? studentId = null,
        CancellationToken cancellationToken = default)
    {
        var date = asOfDate ?? DateTime.UtcNow;
        var overdueInvoices = await _invoiceRepository.GetOverdueInvoicesAsync(date, cancellationToken);

        if (studentId.HasValue)
        {
            overdueInvoices = overdueInvoices.Where(i => i.StudentId == studentId.Value).ToList();
        }

        var buckets = new List<AgingBucketDto>
        {
            new("0-30 days", 0, 30, 0, 0),
            new("31-60 days", 31, 60, 0, 0),
            new("61-90 days", 61, 90, 0, 0),
            new("90+ days", 91, int.MaxValue, 0, 0)
        };

        foreach (var invoice in overdueInvoices)
        {
            var daysOverdue = (date - invoice.DueDate).Days;
            var bucket = buckets.FirstOrDefault(b => daysOverdue >= b.DaysFrom && daysOverdue <= b.DaysTo);
            if (bucket != null)
            {
                var index = buckets.IndexOf(bucket);
                buckets[index] = new AgingBucketDto(
                    bucket.BucketName,
                    bucket.DaysFrom,
                    bucket.DaysTo,
                    bucket.Amount + invoice.BalanceAmount,
                    bucket.InvoiceCount + 1);
            }
        }

        var totalOutstanding = buckets.Sum(b => b.Amount);

        return new AgingReportDto(date, buckets, totalOutstanding);
    }

    public async Task<FeeReconciliationDto> ReconcileFeesAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default)
    {
        // Get all invoices in the date range
        var allInvoices = await _invoiceRepository.GetPendingInvoicesAsync(CancellationToken.None);
        var invoicesInRange = allInvoices
            .Where(i => i.IssueDate >= fromDate && i.IssueDate <= toDate)
            .ToList();

        // Get all payments in the date range
        var allPayments = await _paymentRepository.GetPendingPaymentsAsync(CancellationToken.None);
        var paymentsInRange = allPayments
            .Where(p => p.PaymentDate >= fromDate && p.PaymentDate <= toDate)
            .ToList();

        var totalInvoiced = invoicesInRange.Sum(i => i.TotalAmount);
        var totalPaid = paymentsInRange
            .Where(p => p.Status == PaymentStatus.Completed)
            .Sum(p => p.Amount);
        var totalRefunded = paymentsInRange
            .Where(p => p.Status == PaymentStatus.Refunded)
            .Sum(p => p.Amount);

        var expectedBalance = totalInvoiced - totalPaid + totalRefunded;
        var actualBalance = invoicesInRange
            .Where(i => i.Status != InvoiceStatus.Cancelled)
            .Sum(i => i.BalanceAmount);

        var variance = expectedBalance - actualBalance;
        var isBalanced = Math.Abs(variance) < 0.01m; // Allow for rounding differences

        return new FeeReconciliationDto(
            fromDate,
            toDate,
            totalInvoiced,
            totalPaid,
            totalRefunded,
            expectedBalance,
            actualBalance,
            variance,
            isBalanced);
    }
}

