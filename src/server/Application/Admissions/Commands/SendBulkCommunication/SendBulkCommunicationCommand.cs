using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.SendBulkCommunication;

public sealed record SendBulkCommunicationCommand(
    string Subject,
    string Message,
    CommunicationChannel Channel,
    BulkCommunicationRecipientFilter Filter,
    string? SentBy = null) : IRequest<BulkCommunicationResult>;

public enum CommunicationChannel
{
    Email = 1,
    Sms = 2,
    Both = 3
}

public sealed record BulkCommunicationRecipientFilter(
    List<ApplicationStatus>? Statuses = null,
    bool? IsApplicationSubmitted = null,
    bool? IsPaymentCompleted = null,
    List<Guid>? SpecificAccountIds = null,
    string? SearchTerm = null);

public sealed record BulkCommunicationResult(
    int TotalRecipients,
    int EmailsSent,
    int SmsSent,
    int EmailsFailed,
    int SmsFailed,
    List<BulkCommunicationError> Errors);

public sealed record BulkCommunicationError(
    Guid AccountId,
    string ApplicationNumber,
    string ErrorMessage,
    CommunicationChannel Channel);









