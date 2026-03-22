using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using System.Text.Json;

namespace ERP.Application.Admissions.Queries.GetAdmissionsAnalytics;

public sealed class GetAdmissionsAnalyticsQueryHandler : IRequestHandler<GetAdmissionsAnalyticsQuery, AdmissionsAnalyticsDto>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantApplicationRepository _applicationRepository;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public GetAdmissionsAnalyticsQueryHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantApplicationRepository applicationRepository)
    {
        _accountRepository = accountRepository;
        _applicationRepository = applicationRepository;
    }

    public async Task<AdmissionsAnalyticsDto> Handle(GetAdmissionsAnalyticsQuery request, CancellationToken cancellationToken)
    {
        // Get all accounts
        var (allAccounts, _) = await _accountRepository.GetPagedAsync(
            page: 1,
            pageSize: int.MaxValue,
            isApplicationSubmitted: null,
            isPaymentCompleted: null,
            searchTerm: null,
            cancellationToken: cancellationToken);

        // Apply date filter
        var filteredAccounts = allAccounts
            .Where(a =>
            {
                if (request.FromDate.HasValue && a.CreatedOnUtc < request.FromDate.Value)
                {
                    return false;
                }
                if (request.ToDate.HasValue && a.CreatedOnUtc > request.ToDate.Value)
                {
                    return false;
                }
                return true;
            })
            .ToList();

        var submittedAccounts = filteredAccounts.Where(a => a.IsApplicationSubmitted).ToList();
        var approvedAccounts = submittedAccounts.Where(a => a.Status == ApplicationStatus.Approved).ToList();
        var rejectedAccounts = submittedAccounts.Where(a => a.Status == ApplicationStatus.Rejected).ToList();

        // Calculate trends
        var trends = new ApplicationTrendsDto(
            TotalApplications: filteredAccounts.Count,
            SubmittedThisPeriod: submittedAccounts.Count,
            ApprovedThisPeriod: approvedAccounts.Count,
            RejectedThisPeriod: rejectedAccounts.Count,
            ApprovalRate: submittedAccounts.Any() ? (decimal)approvedAccounts.Count / submittedAccounts.Count * 100 : 0,
            RejectionRate: submittedAccounts.Any() ? (decimal)rejectedAccounts.Count / submittedAccounts.Count * 100 : 0,
            AverageProcessingDays: CalculateAverageProcessingDays(submittedAccounts));

        // Status distribution
        var statusDistribution = new StatusDistributionDto(
            Submitted: submittedAccounts.Count(a => a.Status == ApplicationStatus.Submitted),
            Approved: approvedAccounts.Count,
            Rejected: rejectedAccounts.Count,
            WaitingList: submittedAccounts.Count(a => a.Status == ApplicationStatus.WaitingList),
            EntranceExam: submittedAccounts.Count(a => a.Status == ApplicationStatus.EntranceExam));

        // Payment analytics
        var paidAccounts = filteredAccounts.Where(a => a.IsPaymentCompleted && a.PaymentAmount.HasValue).ToList();
        var paymentAnalytics = new PaymentAnalyticsDto(
            TotalPayments: filteredAccounts.Count,
            PaidCount: paidAccounts.Count,
            PendingCount: filteredAccounts.Count(a => !a.IsPaymentCompleted),
            TotalRevenue: paidAccounts.Sum(a => a.PaymentAmount ?? 0),
            AveragePaymentAmount: paidAccounts.Any() ? paidAccounts.Average(a => a.PaymentAmount ?? 0) : 0,
            PaymentCompletionRate: filteredAccounts.Any() ? (decimal)paidAccounts.Count / filteredAccounts.Count * 100 : 0,
            PaymentTrend: CalculatePaymentTrend(filteredAccounts, request.FromDate, request.ToDate));

        // Document verification stats
        var documentVerification = await CalculateDocumentVerificationStats(submittedAccounts, cancellationToken);

        // Program statistics (simplified - would need program mapping)
        var programStatistics = new ProgramStatisticsDto(
            Programs: new List<ProgramStatDto>()); // TODO: Implement program statistics

        // Daily applications
        var dailyApplications = CalculateDailyApplications(filteredAccounts, request.FromDate, request.ToDate);

        // Monthly applications
        var monthlyApplications = CalculateMonthlyApplications(filteredAccounts, request.FromDate, request.ToDate);

        return new AdmissionsAnalyticsDto(
            DateTime.UtcNow,
            request.FromDate,
            request.ToDate,
            trends,
            statusDistribution,
            paymentAnalytics,
            documentVerification,
            programStatistics,
            dailyApplications,
            monthlyApplications);
    }

    private static int CalculateAverageProcessingDays(List<Domain.Admissions.Entities.StudentApplicantAccount> accounts)
    {
        var accountsWithStatusUpdate = accounts
            .Where(a => a.StatusUpdatedOnUtc > a.CreatedOnUtc)
            .ToList();

        if (!accountsWithStatusUpdate.Any())
        {
            return 0;
        }

        var totalDays = accountsWithStatusUpdate
            .Sum(a => (int)(a.StatusUpdatedOnUtc - a.CreatedOnUtc).TotalDays);

        return totalDays / accountsWithStatusUpdate.Count;
    }

    private static PaymentTrendDto[] CalculatePaymentTrend(
        List<Domain.Admissions.Entities.StudentApplicantAccount> accounts,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var startDate = fromDate ?? accounts.Min(a => a.CreatedOnUtc).Date;
        var endDate = toDate ?? DateTime.UtcNow.Date;

        var trend = new List<PaymentTrendDto>();
        var currentDate = startDate;

        while (currentDate <= endDate)
        {
            var dateAccounts = accounts.Where(a => a.CreatedOnUtc.Date == currentDate).ToList();
            var paid = dateAccounts.Count(a => a.IsPaymentCompleted);
            var pending = dateAccounts.Count(a => !a.IsPaymentCompleted);
            var revenue = dateAccounts.Where(a => a.IsPaymentCompleted && a.PaymentAmount.HasValue)
                .Sum(a => a.PaymentAmount ?? 0);

            trend.Add(new PaymentTrendDto(currentDate, paid, pending, revenue));
            currentDate = currentDate.AddDays(1);
        }

        return trend.ToArray();
    }

    private async Task<DocumentVerificationStatsDto> CalculateDocumentVerificationStats(
        List<Domain.Admissions.Entities.StudentApplicantAccount> accounts,
        CancellationToken cancellationToken)
    {
        int totalDocuments = 0;
        int verifiedCount = 0;
        int pendingCount = 0;
        int rejectedCount = 0;

        foreach (var account in accounts)
        {
            var draft = await _applicationRepository.GetDraftByAccountIdAsync(account.Id, cancellationToken);
            if (draft is null) continue;

            var draftData = JsonSerializer.Deserialize<DTOs.ApplicantApplicationDraftDto>(
                draft.Data, _jsonOptions);

            if (draftData?.DocumentVerificationStatus is null) continue;

            foreach (var verification in draftData.DocumentVerificationStatus.Values)
            {
                totalDocuments++;
                if (verification.IsVerified)
                {
                    verifiedCount++;
                }
                else
                {
                    rejectedCount++;
                }
            }
        }

        // Count pending documents (uploaded but not verified)
        foreach (var account in accounts)
        {
            var draft = await _applicationRepository.GetDraftByAccountIdAsync(account.Id, cancellationToken);
            if (draft is null) continue;

            var draftData = JsonSerializer.Deserialize<DTOs.ApplicantApplicationDraftDto>(
                draft.Data, _jsonOptions);

            if (draftData?.Uploads is null) continue;

            var uploads = new[] {
                draftData.Uploads.StdXMarksheet,
                draftData.Uploads.StdXIIMarksheet,
                draftData.Uploads.CuetMarksheet,
                draftData.Uploads.DifferentlyAbledProof,
                draftData.Uploads.EconomicallyWeakerProof
            };

            foreach (var upload in uploads)
            {
                if (upload != null && !string.IsNullOrWhiteSpace(upload.Data))
                {
                    var docType = GetDocumentType(upload, draftData.Uploads);
                    if (docType != null && (draftData.DocumentVerificationStatus == null ||
                        !draftData.DocumentVerificationStatus.ContainsKey(docType)))
                    {
                        pendingCount++;
                    }
                }
            }
        }

        return new DocumentVerificationStatsDto(
            totalDocuments + pendingCount,
            verifiedCount,
            pendingCount,
            rejectedCount,
            (totalDocuments + pendingCount) > 0 ? (decimal)verifiedCount / (totalDocuments + pendingCount) * 100 : 0);
    }

    private static string? GetDocumentType(
        DTOs.FileAttachmentDto upload,
        DTOs.UploadSection uploads)
    {
        if (upload == uploads.StdXMarksheet) return "StdXMarksheet";
        if (upload == uploads.StdXIIMarksheet) return "StdXIIMarksheet";
        if (upload == uploads.CuetMarksheet) return "CuetMarksheet";
        if (upload == uploads.DifferentlyAbledProof) return "DifferentlyAbledProof";
        if (upload == uploads.EconomicallyWeakerProof) return "EconomicallyWeakerProof";
        return null;
    }

    private static DailyApplicationCountDto[] CalculateDailyApplications(
        List<Domain.Admissions.Entities.StudentApplicantAccount> accounts,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var startDate = fromDate ?? accounts.Min(a => a.CreatedOnUtc).Date;
        var endDate = toDate ?? DateTime.UtcNow.Date;

        var dailyCounts = accounts
            .Where(a => a.CreatedOnUtc.Date >= startDate && a.CreatedOnUtc.Date <= endDate)
            .GroupBy(a => a.CreatedOnUtc.Date)
            .Select(g => new DailyApplicationCountDto(g.Key, g.Count()))
            .OrderBy(d => d.Date)
            .ToArray();

        return dailyCounts;
    }

    private static MonthlyApplicationCountDto[] CalculateMonthlyApplications(
        List<Domain.Admissions.Entities.StudentApplicantAccount> accounts,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var filtered = accounts.AsEnumerable();

        if (fromDate.HasValue)
        {
            filtered = filtered.Where(a => a.CreatedOnUtc >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            filtered = filtered.Where(a => a.CreatedOnUtc <= toDate.Value);
        }

        var monthlyCounts = filtered
            .GroupBy(a => new { a.CreatedOnUtc.Year, a.CreatedOnUtc.Month })
            .Select(g => new MonthlyApplicationCountDto(g.Key.Year, g.Key.Month, g.Count()))
            .OrderBy(m => m.Year)
            .ThenBy(m => m.Month)
            .ToArray();

        return monthlyCounts;
    }
}

