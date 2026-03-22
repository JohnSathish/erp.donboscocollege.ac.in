using ERP.Application.Fees.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Fees.Commands.ProcessPayment;

public sealed class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, ProcessPaymentResult>
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<ProcessPaymentCommandHandler> _logger;

    public ProcessPaymentCommandHandler(
        IPaymentService paymentService,
        ILogger<ProcessPaymentCommandHandler> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var paymentDate = request.PaymentDate ?? DateTime.UtcNow;

            var result = await _paymentService.ProcessPaymentAsync(
                request.InvoiceId,
                request.Amount,
                request.PaymentMethod,
                paymentDate,
                request.PaymentGateway,
                request.TransactionId,
                request.ReferenceNumber,
                request.Remarks,
                request.CreatedBy,
                cancellationToken);

            if (!result.Success)
            {
                return new ProcessPaymentResult(
                    Guid.Empty,
                    string.Empty,
                    null,
                    null,
                    false,
                    result.ErrorMessage);
            }

            // Update cheque details if provided
            if (request.PaymentMethod == Domain.Fees.Entities.PaymentMethod.Cheque &&
                !string.IsNullOrWhiteSpace(request.ChequeNumber) &&
                request.ChequeDate.HasValue &&
                !string.IsNullOrWhiteSpace(request.BankName))
            {
                result.Payment.UpdateChequeDetails(
                    request.ChequeNumber,
                    request.ChequeDate.Value,
                    request.BankName,
                    request.CreatedBy);
            }

            return new ProcessPaymentResult(
                result.Payment.Id,
                result.Payment.PaymentNumber,
                result.Receipt?.Id,
                result.Receipt?.ReceiptNumber,
                true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment for invoice {InvoiceId}.", request.InvoiceId);
            return new ProcessPaymentResult(Guid.Empty, string.Empty, null, null, false, ex.Message);
        }
    }
}

