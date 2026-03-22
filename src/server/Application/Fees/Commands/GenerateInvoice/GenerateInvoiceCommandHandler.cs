using ERP.Application.Fees.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Fees.Commands.GenerateInvoice;

public sealed class GenerateInvoiceCommandHandler : IRequestHandler<GenerateInvoiceCommand, GenerateInvoiceResult>
{
    private readonly IInvoiceService _invoiceService;
    private readonly ILogger<GenerateInvoiceCommandHandler> _logger;

    public GenerateInvoiceCommandHandler(
        IInvoiceService invoiceService,
        ILogger<GenerateInvoiceCommandHandler> logger)
    {
        _invoiceService = invoiceService;
        _logger = logger;
    }

    public async Task<GenerateInvoiceResult> Handle(GenerateInvoiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Domain.Fees.Entities.Invoice invoice;

            if (request.FeeStructureId.HasValue)
            {
                // Generate from fee structure
                invoice = await _invoiceService.GenerateInvoiceFromFeeStructureAsync(
                    request.StudentId,
                    request.FeeStructureId.Value,
                    request.AcademicYear,
                    request.Term,
                    request.DueDate,
                    request.CreatedBy,
                    cancellationToken);
            }
            else if (request.CustomLineItems != null && request.CustomLineItems.Any())
            {
                // Generate custom invoice
                if (request.DueDate == null)
                    return new GenerateInvoiceResult(Guid.Empty, string.Empty, 0, false, "Due date is required for custom invoices.");

                var lineItems = request.CustomLineItems.Select(item => new InvoiceLineItem(
                    item.Description,
                    item.UnitPrice,
                    item.Quantity,
                    item.FeeComponentId)).ToList();

                invoice = await _invoiceService.GenerateCustomInvoiceAsync(
                    request.StudentId,
                    request.AcademicYear,
                    lineItems,
                    request.DueDate.Value,
                    request.Term,
                    request.CreatedBy,
                    cancellationToken);
            }
            else
            {
                return new GenerateInvoiceResult(Guid.Empty, string.Empty, 0, false, "Either FeeStructureId or CustomLineItems must be provided.");
            }

            return new GenerateInvoiceResult(
                invoice.Id,
                invoice.InvoiceNumber,
                invoice.TotalAmount,
                true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating invoice for student {StudentId}.", request.StudentId);
            return new GenerateInvoiceResult(Guid.Empty, string.Empty, 0, false, ex.Message);
        }
    }
}

