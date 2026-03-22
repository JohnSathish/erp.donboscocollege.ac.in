using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IApplicantNotificationService
{
    Task SendRegistrationNotificationsAsync(
        string fullName,
        string uniqueId,
        string email,
        string mobileNumber,
        string temporaryPassword,
        CancellationToken cancellationToken = default);

    Task SendPasswordChangeNotificationAsync(
        string fullName,
        string email,
        CancellationToken cancellationToken = default);

    Task SendPasswordResetNotificationAsync(
        string fullName,
        string email,
        string mobileNumber,
        string applicationNumber,
        string temporaryPassword,
        string resetBy,
        CancellationToken cancellationToken = default);

    Task SendStatusUpdateNotificationAsync(
        string fullName,
        string email,
        string mobileNumber,
        string applicationNumber,
        ApplicationStatus status,
        string? remarks,
        DateTime? entranceExamScheduledOnUtc,
        string? entranceExamVenue,
        string? entranceExamInstructions,
        string? majorSubject = null,
        DateTime? paymentDeadlineUtc = null,
        CancellationToken cancellationToken = default);

    Task SendApplicationSubmissionNotificationAsync(
        string fullName,
        string email,
        string applicationNumber,
        byte[] pdfContent,
        string pdfFileName,
        CancellationToken cancellationToken = default);

    Task SendEnrollmentNotificationAsync(
        string fullName,
        string email,
        string mobileNumber,
        string applicationNumber,
        DateTime enrolledOnUtc,
        string? remarks,
        CancellationToken cancellationToken = default);

    Task SendBulkEmailAsync(
        string fullName,
        string email,
        string subject,
        string message,
        CancellationToken cancellationToken = default);

    Task SendBulkSmsAsync(
        string mobileNumber,
        string message,
        CancellationToken cancellationToken = default);

    Task SendAdmissionOfferNotificationAsync(
        string fullName,
        string email,
        string mobileNumber,
        string applicationNumber,
        int meritRank,
        string shift,
        string majorSubject,
        DateTime offerDate,
        DateTime expiryDate,
        CancellationToken cancellationToken = default);

    Task SendOfferAcceptedNotificationAsync(
        string fullName,
        string email,
        string mobileNumber,
        string applicationNumber,
        DateTime acceptedOnUtc,
        CancellationToken cancellationToken = default);

    Task SendOfferRejectedNotificationAsync(
        string fullName,
        string email,
        string mobileNumber,
        string applicationNumber,
        DateTime rejectedOnUtc,
        string? reason,
        CancellationToken cancellationToken = default);

    Task SendAdmissionFeePaymentNotificationAsync(
        string fullName,
        string email,
        string mobileNumber,
        string applicationNumber,
        decimal classXIIPercentage,
        string shift,
        string majorSubject,
        decimal admissionFeeAmount,
        DateTime offerDate,
        DateTime expiryDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sent once after online application fee (Razorpay) is verified successfully.
    /// </summary>
    Task SendApplicationFeePaidConfirmationAsync(
        string fullName,
        string email,
        string applicationNumber,
        decimal amountPaid,
        string transactionId,
        DateTime paidOnUtc,
        CancellationToken cancellationToken = default);
}

