using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListPayments;

public sealed record ListPaymentsQuery(
    int Page = 1,
    int PageSize = 50,
    bool? IsPaymentCompleted = null,
    string? SearchTerm = null,
    DateTime? PaymentDateFrom = null,
    DateTime? PaymentDateTo = null,
    decimal? MinAmount = null,
    decimal? MaxAmount = null) : IRequest<PaymentsListResponse>;

public sealed record PaymentsListResponse(
    IReadOnlyCollection<PaymentDto> Payments,
    int TotalCount,
    int Page,
    int PageSize);











