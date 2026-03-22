using ERP.Application.Admissions.Interfaces;
using ERP.Application.Fees.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Domain.Fees.Entities;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Fees;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IFeeStructureRepository _feeStructureRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IScholarshipRepository _scholarshipRepository;
    private readonly ILogger<InvoiceService> _logger;

    public InvoiceService(
        IInvoiceRepository invoiceRepository,
        IFeeStructureRepository feeStructureRepository,
        IStudentRepository studentRepository,
        IScholarshipRepository scholarshipRepository,
        ILogger<InvoiceService> logger)
    {
        _invoiceRepository = invoiceRepository;
        _feeStructureRepository = feeStructureRepository;
        _studentRepository = studentRepository;
        _scholarshipRepository = scholarshipRepository;
        _logger = logger;
    }

    public async Task<Invoice> GenerateInvoiceFromFeeStructureAsync(
        Guid studentId,
        Guid feeStructureId,
        string academicYear,
        string? term = null,
        DateTime? dueDate = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student == null)
            throw new InvalidOperationException($"Student with ID {studentId} not found.");

        var feeStructure = await _feeStructureRepository.GetByIdAsync(feeStructureId, cancellationToken);
        if (feeStructure == null)
            throw new InvalidOperationException($"Fee structure with ID {feeStructureId} not found.");

        if (!feeStructure.IsActive)
            throw new InvalidOperationException($"Fee structure {feeStructure.Name} is not active.");

        var invoiceNumber = await GenerateInvoiceNumberAsync(academicYear, cancellationToken);
        var invoiceDueDate = dueDate ?? DateTime.UtcNow.AddDays(30);

        var invoice = new Invoice(
            studentId,
            invoiceNumber,
            invoiceDueDate,
            academicYear,
            feeStructureId,
            term,
            null,
            createdBy);

        // Add invoice lines from fee components
        foreach (var component in feeStructure.Components.OrderBy(c => c.DisplayOrder))
        {
            var line = new InvoiceLine(
                invoice.Id,
                component.Name,
                component.Amount,
                1,
                component.Id,
                component.DisplayOrder,
                createdBy);

            invoice.AddLine(line);
        }

        var createdInvoice = await _invoiceRepository.AddAsync(invoice, cancellationToken);

        // Apply scholarships after invoice is created
        await ApplyScholarshipsToInvoiceAsync(createdInvoice.Id, cancellationToken);
        
        // Reload invoice to get updated scholarship amounts
        createdInvoice = await _invoiceRepository.GetByIdAsync(createdInvoice.Id, cancellationToken) 
            ?? throw new InvalidOperationException("Failed to reload invoice after scholarship application.");

        _logger.LogInformation("Invoice {InvoiceNumber} generated for student {StudentNumber} (ID: {StudentId}) from fee structure {FeeStructureName}.",
            invoiceNumber, student.StudentNumber, studentId, feeStructure.Name);

        return createdInvoice;
    }

    public async Task<Invoice> GenerateCustomInvoiceAsync(
        Guid studentId,
        string academicYear,
        IReadOnlyCollection<InvoiceLineItem> lineItems,
        DateTime dueDate,
        string? term = null,
        string? createdBy = null,
        CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student == null)
            throw new InvalidOperationException($"Student with ID {studentId} not found.");

        if (lineItems == null || !lineItems.Any())
            throw new ArgumentException("At least one line item is required.", nameof(lineItems));

        var invoiceNumber = await GenerateInvoiceNumberAsync(academicYear, cancellationToken);

        var invoice = new Invoice(
            studentId,
            invoiceNumber,
            dueDate,
            academicYear,
            null,
            term,
            null,
            createdBy);

        int displayOrder = 0;
        foreach (var item in lineItems)
        {
            var line = new InvoiceLine(
                invoice.Id,
                item.Description,
                item.UnitPrice,
                item.Quantity,
                item.FeeComponentId,
                displayOrder++,
                createdBy);

            invoice.AddLine(line);
        }

        var createdInvoice = await _invoiceRepository.AddAsync(invoice, cancellationToken);

        // Apply scholarships after invoice is created
        await ApplyScholarshipsToInvoiceAsync(createdInvoice.Id, cancellationToken);
        
        // Reload invoice to get updated scholarship amounts
        createdInvoice = await _invoiceRepository.GetByIdAsync(createdInvoice.Id, cancellationToken) 
            ?? throw new InvalidOperationException("Failed to reload invoice after scholarship application.");

        _logger.LogInformation("Custom invoice {InvoiceNumber} generated for student {StudentNumber} (ID: {StudentId}).",
            invoiceNumber, student.StudentNumber, studentId);

        return createdInvoice;
    }

    public async Task ApplyScholarshipsToInvoiceAsync(
        Guid invoiceId,
        CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdForUpdateAsync(invoiceId, cancellationToken);
        if (invoice == null)
            throw new InvalidOperationException($"Invoice with ID {invoiceId} not found.");

        if (invoice.Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Scholarships can only be applied to draft invoices.");

        var scholarships = await _scholarshipRepository.GetActiveScholarshipsByStudentIdAsync(
            invoice.StudentId,
            invoice.IssueDate,
            cancellationToken);

        decimal totalScholarshipAmount = 0;
        foreach (var scholarship in scholarships)
        {
            var discountAmount = scholarship.CalculateDiscountAmount(invoice.SubTotal - invoice.DiscountAmount);
            totalScholarshipAmount += discountAmount;
        }

        if (totalScholarshipAmount > 0)
        {
            invoice.ApplyScholarship(totalScholarshipAmount);
            await _invoiceRepository.UpdateAsync(invoice, cancellationToken);

            _logger.LogInformation("Applied scholarship amount {Amount} to invoice {InvoiceNumber}.",
                totalScholarshipAmount, invoice.InvoiceNumber);
        }
    }

    public async Task<string> GenerateInvoiceNumberAsync(
        string academicYear,
        CancellationToken cancellationToken = default)
    {
        var yearPrefix = academicYear.Replace("-", "").Substring(0, 4); // e.g., "2024" from "2024-2025"
        var prefix = $"INV-{yearPrefix}-";

        // Get the last invoice number for this year
        var lastInvoice = await _invoiceRepository.GetByInvoiceNumberAsync($"{prefix}00001", cancellationToken);
        // In a real implementation, you'd query for the highest number
        // For now, we'll use a simple timestamp-based approach
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        return $"{prefix}{timestamp.Substring(timestamp.Length - 6)}";
    }
}

