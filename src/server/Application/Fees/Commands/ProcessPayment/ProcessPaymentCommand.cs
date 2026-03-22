using MediatR;
using ERP.Domain.Fees.Entities;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.Fees.Commands.ProcessPayment;

public sealed record ProcessPaymentCommand(
    Guid InvoiceId,
    [Required] decimal Amount,
    [Required] PaymentMethod PaymentMethod,
    DateTime? PaymentDate = null,
    string? PaymentGateway = null,
    string? TransactionId = null,
    string? ReferenceNumber = null,
    string? ChequeNumber = null,
    DateTime? ChequeDate = null,
    string? BankName = null,
    string? Remarks = null,
    string? CreatedBy = null) : IRequest<ProcessPaymentResult>;

public sealed record ProcessPaymentResult(
    Guid PaymentId,
    string PaymentNumber,
    Guid? ReceiptId,
    string? ReceiptNumber,
    bool Success,
    string? ErrorMessage = null);

