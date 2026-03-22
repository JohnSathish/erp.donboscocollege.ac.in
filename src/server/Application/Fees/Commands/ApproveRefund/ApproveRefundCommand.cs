using MediatR;

namespace ERP.Application.Fees.Commands.ApproveRefund;

public sealed record ApproveRefundCommand(
    Guid RefundId,
    string? Remarks = null,
    string? ApprovedBy = null) : IRequest<bool>;

