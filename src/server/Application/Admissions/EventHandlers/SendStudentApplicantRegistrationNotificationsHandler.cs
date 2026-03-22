using ERP.Application.Admissions.Events;
using ERP.Application.Admissions.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.EventHandlers;

public sealed class SendStudentApplicantRegistrationNotificationsHandler : INotificationHandler<StudentApplicantRegisteredEvent>
{
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<SendStudentApplicantRegistrationNotificationsHandler> _logger;

    public SendStudentApplicantRegistrationNotificationsHandler(
        IApplicantNotificationService notificationService,
        ILogger<SendStudentApplicantRegistrationNotificationsHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Handle(StudentApplicantRegisteredEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Dispatching registration notifications for applicant {UniqueId} ({Email})",
                notification.UniqueId,
                notification.Email);

            await _notificationService.SendRegistrationNotificationsAsync(
                notification.FullName,
                notification.UniqueId,
                notification.Email,
                notification.MobileNumber,
                notification.TemporaryPassword,
                cancellationToken);
            
            _logger.LogInformation(
                "Registration notifications sent successfully for applicant {UniqueId} ({Email})",
                notification.UniqueId,
                notification.Email);
        }
        catch (Exception ex)
        {
            // Log the error but don't throw - registration should succeed even if notifications fail
            _logger.LogError(
                ex,
                "Failed to send registration notifications for applicant {UniqueId} ({Email}). Registration succeeded but notifications were not sent.",
                notification.UniqueId,
                notification.Email);
        }
    }
}

