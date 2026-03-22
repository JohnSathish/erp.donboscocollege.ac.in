using ERP.Application.Fees.Interfaces;
using ERP.Domain.Fees.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Fees.Queries.GetStudentInvoices;

public sealed class GetStudentInvoicesQueryHandler : IRequestHandler<GetStudentInvoicesQuery, IReadOnlyCollection<InvoiceSummaryDto>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger<GetStudentInvoicesQueryHandler> _logger;

    public GetStudentInvoicesQueryHandler(
        IInvoiceRepository invoiceRepository,
        ILogger<GetStudentInvoicesQueryHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<InvoiceSummaryDto>> Handle(GetStudentInvoicesQuery request, CancellationToken cancellationToken)
    {
        var invoices = await _invoiceRepository.GetByStudentIdAsync(request.StudentId, cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.AcademicYear))
        {
            invoices = invoices.Where(i => i.AcademicYear == request.AcademicYear).ToList();
        }

        return invoices.Select(i => new InvoiceSummaryDto(
            i.Id,
            i.InvoiceNumber,
            i.IssueDate,
            i.DueDate,
            i.TotalAmount,
            i.PaidAmount,
            i.BalanceAmount,
            i.Status)).ToList();
    }
}

