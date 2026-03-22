using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.UpdateApplicantStatus;

public sealed class UpdateApplicantStatusCommandHandler : IRequestHandler<UpdateApplicantStatusCommand, ApplicantStatusUpdatedResult>
{
    private readonly IAdmissionsRepository _repository;
    private readonly IApplicantNotificationService _notificationService;

    public UpdateApplicantStatusCommandHandler(
        IAdmissionsRepository repository,
        IApplicantNotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public async Task<ApplicantStatusUpdatedResult> Handle(UpdateApplicantStatusCommand request, CancellationToken cancellationToken)
    {
        var applicant = await _repository.GetApplicantByIdAsync(request.ApplicantId, cancellationToken);
        if (applicant is null)
        {
            throw new InvalidOperationException($"Applicant {request.ApplicantId} was not found.");
        }

        var entranceExam = request.EntranceExam;
        if (request.Status == ApplicationStatus.EntranceExam && entranceExam is null)
        {
            throw new InvalidOperationException("Entrance exam details are required when setting status to EntranceExam.");
        }

        applicant.UpdateStatus(
            request.Status,
            request.UpdatedBy,
            DateTime.UtcNow,
            request.Remarks,
            entranceExam?.ScheduledOnUtc,
            entranceExam?.Venue,
            entranceExam?.Instructions);

        await _repository.SaveChangesAsync(cancellationToken);

        if (request.NotifyApplicant)
        {
            await _notificationService.SendStatusUpdateNotificationAsync(
                $"{applicant.FirstName} {applicant.LastName}".Trim(),
                applicant.Email,
                applicant.MobileNumber,
                applicant.ApplicationNumber,
                request.Status,
                request.Remarks,
                entranceExam?.ScheduledOnUtc,
                entranceExam?.Venue,
                entranceExam?.Instructions,
                null, // majorSubject - not available in this context
                null, // paymentDeadlineUtc - not available in this context
                cancellationToken);
        }

        return new ApplicantStatusUpdatedResult(applicant.Id);
    }
}

