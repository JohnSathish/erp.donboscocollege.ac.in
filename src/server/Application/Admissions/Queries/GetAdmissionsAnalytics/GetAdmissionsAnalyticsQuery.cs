using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetAdmissionsAnalytics;

public sealed record GetAdmissionsAnalyticsQuery(
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IRequest<AdmissionsAnalyticsDto>;

public sealed record AdmissionsAnalyticsDto(
    DateTime GeneratedOnUtc,
    DateTime? FromDate,
    DateTime? ToDate,
    ApplicationTrendsDto Trends,
    StatusDistributionDto StatusDistribution,
    PaymentAnalyticsDto PaymentAnalytics,
    DocumentVerificationStatsDto DocumentVerification,
    ProgramStatisticsDto ProgramStatistics,
    DailyApplicationCountDto[] DailyApplications,
    MonthlyApplicationCountDto[] MonthlyApplications);

public sealed record ApplicationTrendsDto(
    int TotalApplications,
    int SubmittedThisPeriod,
    int ApprovedThisPeriod,
    int RejectedThisPeriod,
    decimal ApprovalRate,
    decimal RejectionRate,
    int AverageProcessingDays);

public sealed record StatusDistributionDto(
    int Submitted,
    int Approved,
    int Rejected,
    int WaitingList,
    int EntranceExam);

public sealed record PaymentAnalyticsDto(
    int TotalPayments,
    int PaidCount,
    int PendingCount,
    decimal TotalRevenue,
    decimal AveragePaymentAmount,
    decimal PaymentCompletionRate,
    PaymentTrendDto[] PaymentTrend);

public sealed record PaymentTrendDto(
    DateTime Date,
    int PaidCount,
    int PendingCount,
    decimal Revenue);

public sealed record DocumentVerificationStatsDto(
    int TotalDocuments,
    int VerifiedCount,
    int PendingCount,
    int RejectedCount,
    decimal VerificationRate);

public sealed record ProgramStatisticsDto(
    IReadOnlyCollection<ProgramStatDto> Programs);

public sealed record ProgramStatDto(
    string ProgramCode,
    string ProgramName,
    int ApplicationCount,
    int ApprovedCount,
    decimal ApprovalRate);

public sealed record DailyApplicationCountDto(
    DateTime Date,
    int Count);

public sealed record MonthlyApplicationCountDto(
    int Year,
    int Month,
    int Count);











