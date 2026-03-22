using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Queries.ListPayments;

public sealed class ListPaymentsQueryHandler : IRequestHandler<ListPaymentsQuery, PaymentsListResponse>
{
    private readonly IApplicantAccountRepository _accountRepository;

    public ListPaymentsQueryHandler(IApplicantAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<PaymentsListResponse> Handle(ListPaymentsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var (accounts, totalCount) = await _accountRepository.GetPagedAsync(
            page,
            pageSize,
            isApplicationSubmitted: null, // Get all regardless of submission status
            isPaymentCompleted: request.IsPaymentCompleted,
            searchTerm: request.SearchTerm,
            cancellationToken: cancellationToken);

        // Apply additional filters in memory (since repository doesn't support all filters)
        var filteredAccounts = accounts
            .Where(a =>
            {
                if (request.PaymentDateFrom.HasValue && a.PaymentCompletedOnUtc.HasValue)
                {
                    if (a.PaymentCompletedOnUtc.Value < request.PaymentDateFrom.Value)
                        return false;
                }

                if (request.PaymentDateTo.HasValue && a.PaymentCompletedOnUtc.HasValue)
                {
                    if (a.PaymentCompletedOnUtc.Value > request.PaymentDateTo.Value)
                        return false;
                }

                if (request.MinAmount.HasValue && a.PaymentAmount.HasValue)
                {
                    if (a.PaymentAmount.Value < request.MinAmount.Value)
                        return false;
                }

                if (request.MaxAmount.HasValue && a.PaymentAmount.HasValue)
                {
                    if (a.PaymentAmount.Value > request.MaxAmount.Value)
                        return false;
                }

                return true;
            })
            .OrderByDescending(a => a.PaymentCompletedOnUtc ?? a.CreatedOnUtc)
            .ToList();

        var payments = filteredAccounts
            .Select(account => new PaymentDto(
                account.Id,
                account.UniqueId,
                account.FullName,
                account.Email,
                account.MobileNumber,
                account.IsPaymentCompleted,
                account.PaymentOrderId,
                account.PaymentTransactionId,
                account.PaymentAmount,
                account.PaymentCompletedOnUtc,
                account.CreatedOnUtc,
                account.Status))
            .ToList();

        // Recalculate total count after filtering
        var filteredTotalCount = payments.Count;

        return new PaymentsListResponse(payments, filteredTotalCount, page, pageSize);
    }
}

