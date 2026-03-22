using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using ERP.Domain.Admissions.Entities;
using MediatR;
using System.Linq;
using AdmissionChannelEntity = ERP.Domain.Admissions.Entities.AdmissionChannel;

namespace ERP.Application.Admissions.Queries.GetAdminDashboard;

public sealed class GetAdminDashboardQueryHandler : IRequestHandler<GetAdminDashboardQuery, AdminDashboardDto>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IOfflineFormIssuanceRepository _issuanceRepository;

    public GetAdminDashboardQueryHandler(
        IApplicantAccountRepository accountRepository,
        IOfflineFormIssuanceRepository issuanceRepository)
    {
        _accountRepository = accountRepository;
        _issuanceRepository = issuanceRepository;
    }

    public async Task<AdminDashboardDto> Handle(GetAdminDashboardQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetAllAsync(cancellationToken);
        var accountsList = accounts.ToList();

        var totalApplications = accountsList.Count;
        var submittedApplications = accountsList.Count(x => x.IsApplicationSubmitted);
        var pendingApplications = accountsList.Count(x => !x.IsApplicationSubmitted);
        var approvedApplications = accountsList.Count(x => x.Status == ApplicationStatus.Approved);
        var rejectedApplications = accountsList.Count(x => x.Status == ApplicationStatus.Rejected);
        var waitingListApplications = accountsList.Count(x => x.Status == ApplicationStatus.WaitingList);
        var entranceExamApplications = accountsList.Count(x => x.Status == ApplicationStatus.EntranceExam);
        var paidApplications = accountsList.Count(x => x.IsPaymentCompleted);
        var unpaidApplications = accountsList.Count(x => !x.IsPaymentCompleted);
        var totalRevenue = accountsList.Where(x => x.PaymentAmount.HasValue).Sum(x => x.PaymentAmount!.Value);

        var pendingPipelineCount = accountsList.Count(x =>
            x.IsApplicationSubmitted &&
            x.Status is not ApplicationStatus.Approved
                and not ApplicationStatus.Rejected
                and not ApplicationStatus.Enrolled);

        var statisticsByStatus = new StatisticsByStatus(
            accountsList.Count(x => x.Status == ApplicationStatus.Submitted),
            approvedApplications,
            rejectedApplications,
            waitingListApplications,
            entranceExamApplications);

        var offlineIssued = await _issuanceRepository.CountAsync(cancellationToken);
        var offlineReceived = accountsList.Count(x =>
            x.AdmissionChannel == AdmissionChannelEntity.Offline && x.OfflineFormReceivedOnUtc.HasValue);
        var offlineSubmitted = accountsList.Count(x =>
            x.AdmissionChannel == AdmissionChannelEntity.Offline && x.IsApplicationSubmitted);

        return new AdminDashboardDto(
            totalApplications,
            submittedApplications,
            pendingApplications,
            approvedApplications,
            rejectedApplications,
            waitingListApplications,
            entranceExamApplications,
            paidApplications,
            unpaidApplications,
            pendingPipelineCount,
            totalRevenue,
            statisticsByStatus,
            offlineIssued,
            offlineReceived,
            offlineSubmitted);
    }
}
