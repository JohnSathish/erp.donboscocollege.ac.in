using MediatR;
using ERP.Domain.Fees.Entities;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Fees.Commands.CreateRefundRequest;

public sealed record CreateRefundRequestCommand(
    Guid PaymentId,
    [Required] decimal Amount,
    [Required] RefundReason Reason,
    string? ReasonDetails = null,
    string? Remarks = null,
    string? CreatedBy = null) : IRequest<Guid>;

