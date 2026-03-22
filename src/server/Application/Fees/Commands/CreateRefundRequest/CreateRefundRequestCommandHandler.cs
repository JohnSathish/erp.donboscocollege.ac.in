using ERP.Application.Fees.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Fees.Commands.CreateRefundRequest;

public sealed class CreateRefundRequestCommandHandler : IRequestHandler<CreateRefundRequestCommand, Guid>
{
    private readonly IRefundService _refundService;
    private readonly ILogger<CreateRefundRequestCommandHandler> _logger;

    public CreateRefundRequestCommandHandler(
        IRefundService refundService,
        ILogger<CreateRefundRequestCommandHandler> logger)
    {
        _refundService = refundService;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateRefundRequestCommand request, CancellationToken cancellationToken)
    {
        var refund = await _refundService.CreateRefundRequestAsync(
            request.PaymentId,
            request.Amount,
            request.Reason,
            request.ReasonDetails,
            request.Remarks,
            request.CreatedBy,
            cancellationToken);

        _logger.LogInformation("Refund request {RefundNumber} created for payment {PaymentId}.", refund.RefundNumber, request.PaymentId);
        return refund.Id;
    }
}

