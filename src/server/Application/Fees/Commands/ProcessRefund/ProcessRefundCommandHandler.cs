using ERP.Application.Fees.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Fees.Commands.ProcessRefund;

public sealed class ProcessRefundCommandHandler : IRequestHandler<ProcessRefundCommand, bool>
{
    private readonly IRefundService _refundService;
    private readonly ILogger<ProcessRefundCommandHandler> _logger;

    public ProcessRefundCommandHandler(
        IRefundService refundService,
        ILogger<ProcessRefundCommandHandler> logger)
    {
        _refundService = refundService;
        _logger = logger;
    }

    public async Task<bool> Handle(ProcessRefundCommand request, CancellationToken cancellationToken)
    {
        var result = await _refundService.ProcessRefundAsync(
            request.RefundId,
            request.ProcessedBy ?? "System",
            request.Remarks,
            cancellationToken);

        if (result)
        {
            _logger.LogInformation("Refund {RefundId} processed by {ProcessedBy}.", request.RefundId, request.ProcessedBy);
        }

        return result;
    }
}

