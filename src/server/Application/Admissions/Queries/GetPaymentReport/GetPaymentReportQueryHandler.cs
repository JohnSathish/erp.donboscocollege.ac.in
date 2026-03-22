using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetPaymentReport;

public sealed class GetPaymentReportQueryHandler : IRequestHandler<GetPaymentReportQuery, PaymentReportDto>
{
    private readonly IApplicantAccountRepository _accountRepository;

    public GetPaymentReportQueryHandler(IApplicantAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<PaymentReportDto> Handle(GetPaymentReportQuery request, CancellationToken cancellationToken)
    {
        // Get all accounts (we'll filter in memory for date ranges)
        var (accounts, _) = await _accountRepository.GetPagedAsync(
            page: 1,
            pageSize: int.MaxValue,
            isApplicationSubmitted: null,
            isPaymentCompleted: request.IsPaymentCompleted,
            searchTerm: null,
            cancellationToken: cancellationToken);

        // Apply date filters
        var filteredAccounts = accounts
            .Where(a =>
            {
                var paymentDate = a.PaymentCompletedOnUtc ?? a.CreatedOnUtc;

                if (request.FromDate.HasValue && paymentDate < request.FromDate.Value)
                {
                    return false;
                }

                if (request.ToDate.HasValue && paymentDate > request.ToDate.Value)
                {
                    return false;
                }

                return true;
            })
            .OrderByDescending(a => a.PaymentCompletedOnUtc ?? a.CreatedOnUtc)
            .ToList();

        var paidAccounts = filteredAccounts.Where(a => a.IsPaymentCompleted && a.PaymentAmount.HasValue).ToList();
        var pendingAccounts = filteredAccounts.Where(a => !a.IsPaymentCompleted).ToList();

        var summary = new PaymentReportSummary(
            TotalPayments: filteredAccounts.Count,
            PaidCount: paidAccounts.Count,
            PendingCount: pendingAccounts.Count,
            TotalRevenue: paidAccounts.Sum(a => a.PaymentAmount ?? 0),
            AveragePaymentAmount: paidAccounts.Any() ? paidAccounts.Average(a => a.PaymentAmount ?? 0) : 0,
            MinPaymentAmount: paidAccounts.Any() ? paidAccounts.Min(a => a.PaymentAmount ?? 0) : 0,
            MaxPaymentAmount: paidAccounts.Any() ? paidAccounts.Max(a => a.PaymentAmount ?? 0) : 0);

        var items = filteredAccounts
            .Select(account => new PaymentReportItemDto(
                account.UniqueId,
                account.FullName,
                account.Email,
                account.MobileNumber,
                account.IsPaymentCompleted,
                account.PaymentAmount,
                account.PaymentTransactionId,
                account.PaymentCompletedOnUtc,
                account.CreatedOnUtc,
                account.Status.ToString()))
            .ToList();

        return new PaymentReportDto(
            DateTime.UtcNow,
            request.FromDate,
            request.ToDate,
            summary,
            items);
    }
}











