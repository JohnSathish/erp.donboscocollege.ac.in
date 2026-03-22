using MediatR;

namespace ERP.Application.Fees.Commands.PublishInvoice;

public sealed record PublishInvoiceCommand(
    Guid InvoiceId,
    string? UpdatedBy = null) : IRequest<bool>;

