namespace ERP.Application.Admissions.ViewModels;

public sealed record AdminDashboardDto(
    int TotalApplications,
    int SubmittedApplications,
    int PendingApplications,
    int ApprovedApplications,
    int RejectedApplications,
    int WaitingListApplications,
    int EntranceExamApplications,
    int PaidApplications,
    int UnpaidApplications,
    /// <summary>Submitted applications not yet Approved, Rejected, or Enrolled (admission pipeline).</summary>
    int PendingPipelineCount,
    decimal TotalRevenue,
    StatisticsByStatus StatisticsByStatus,
    int OfflineFormsIssued,
    int OfflineFormsReceived,
    int OfflineApplicationsSubmitted);

public sealed record StatisticsByStatus(
    int Submitted,
    int Approved,
    int Rejected,
    int WaitingList,
    int EntranceExam);














