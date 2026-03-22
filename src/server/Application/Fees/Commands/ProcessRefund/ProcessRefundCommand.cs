using MediatR;

namespace ERP.Application.Fees.Commands.ProcessRefund;

public sealed record ProcessRefundCommand(
    Guid RefundId,
    string? Remarks = null,
    string? ProcessedBy = null) : IRequest<bool>;

