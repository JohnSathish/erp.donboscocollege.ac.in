using ERP.Application.Fees.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Fees.Commands.ApproveRefund;

public sealed class ApproveRefundCommandHandler : IRequestHandler<ApproveRefundCommand, bool>
{
    private readonly IRefundService _refundService;
    private readonly ILogger<ApproveRefundCommandHandler> _logger;

    public ApproveRefundCommandHandler(
        IRefundService refundService,
        ILogger<ApproveRefundCommandHandler> logger)
    {
        _refundService = refundService;
        _logger = logger;
    }

    public async Task<bool> Handle(ApproveRefundCommand request, CancellationToken cancellationToken)
    {
        var result = await _refundService.ApproveRefundAsync(
            request.RefundId,
            request.ApprovedBy ?? "System",
            request.Remarks,
            cancellationToken);

        if (result)
        {
            _logger.LogInformation("Refund {RefundId} approved by {ApprovedBy}.", request.RefundId, request.ApprovedBy);
        }

        return result;
    }
}

