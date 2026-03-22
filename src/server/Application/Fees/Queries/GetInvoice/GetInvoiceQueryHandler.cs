using ERP.Application.Fees.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Fees.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Fees.Queries.GetInvoice;

public sealed class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, InvoiceDto?>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<GetInvoiceQueryHandler> _logger;

    public GetInvoiceQueryHandler(
        IInvoiceRepository invoiceRepository,
        IStudentRepository studentRepository,
        ILogger<GetInvoiceQueryHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<InvoiceDto?> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken);
        if (invoice == null)
        {
            _logger.LogWarning("Invoice with ID {InvoiceId} not found.", request.InvoiceId);
            return null;
        }

        var student = await _studentRepository.GetByIdAsync(invoice.StudentId, cancellationToken);
        if (student == null)
        {
            _logger.LogWarning("Student with ID {StudentId} not found for invoice {InvoiceId}.", invoice.StudentId, request.InvoiceId);
            return null;
        }

        var lineDtos = invoice.Lines.Select(l => new InvoiceLineDto(
            l.Id,
            l.FeeComponentId,
            l.Description,
            l.Quantity,
            l.UnitPrice,
            l.Amount,
            l.DisplayOrder)).ToList();

        return new InvoiceDto(
            invoice.Id,
            invoice.StudentId,
            student.StudentNumber,
            student.FullName,
            invoice.InvoiceNumber,
            invoice.FeeStructureId,
            invoice.AcademicYear,
            invoice.Term,
            invoice.IssueDate,
            invoice.DueDate,
            invoice.SubTotal,
            invoice.DiscountAmount,
            invoice.ScholarshipAmount,
            invoice.TaxAmount,
            invoice.TotalAmount,
            invoice.PaidAmount,
            invoice.BalanceAmount,
            invoice.Status,
            invoice.Remarks,
            lineDtos);
    }
}

