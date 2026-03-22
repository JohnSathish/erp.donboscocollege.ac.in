using ERP.Application.Fees.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Fees.Commands.PublishInvoice;

public sealed class PublishInvoiceCommandHandler : IRequestHandler<PublishInvoiceCommand, bool>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger<PublishInvoiceCommandHandler> _logger;

    public PublishInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        ILogger<PublishInvoiceCommandHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(PublishInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdForUpdateAsync(request.InvoiceId, cancellationToken);
        if (invoice == null)
        {
            _logger.LogWarning("Invoice with ID {InvoiceId} not found for publishing.", request.InvoiceId);
            return false;
        }

        try
        {
            invoice.Publish(request.UpdatedBy);
            await _invoiceRepository.UpdateAsync(invoice, cancellationToken);

            _logger.LogInformation("Invoice {InvoiceNumber} published.", invoice.InvoiceNumber);
            return true;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Cannot publish invoice {InvoiceId}: {Message}", request.InvoiceId, ex.Message);
            return false;
        }
    }
}

