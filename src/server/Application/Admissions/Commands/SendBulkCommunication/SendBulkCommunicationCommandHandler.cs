using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.SendBulkCommunication;

public sealed class SendBulkCommunicationCommandHandler : IRequestHandler<SendBulkCommunicationCommand, BulkCommunicationResult>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<SendBulkCommunicationCommandHandler> _logger;

    public SendBulkCommunicationCommandHandler(
        IApplicantAccountRepository accountRepository,
        IApplicantNotificationService notificationService,
        ILogger<SendBulkCommunicationCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<BulkCommunicationResult> Handle(SendBulkCommunicationCommand request, CancellationToken cancellationToken)
    {
        // Get recipients based on filter
        var recipients = await GetRecipientsAsync(request.Filter, cancellationToken);

        if (recipients.Count == 0)
        {
            _logger.LogWarning("No recipients found matching the filter criteria.");
            return new BulkCommunicationResult(0, 0, 0, 0, 0, new List<BulkCommunicationError>());
        }

        _logger.LogInformation(
            "Sending bulk communication to {RecipientCount} recipients via {Channel}",
            recipients.Count,
            request.Channel);

        var emailsSent = 0;
        var smsSent = 0;
        var emailsFailed = 0;
        var smsFailed = 0;
        var errors = new List<BulkCommunicationError>();

        foreach (var recipient in recipients)
        {
            try
            {
                if (request.Channel == CommunicationChannel.Email || request.Channel == CommunicationChannel.Both)
                {
                    try
                    {
                        await _notificationService.SendBulkEmailAsync(
                            recipient.FullName,
                            recipient.Email,
                            request.Subject,
                            request.Message,
                            cancellationToken);
                        emailsSent++;
                    }
                    catch (Exception ex)
                    {
                        emailsFailed++;
                        errors.Add(new BulkCommunicationError(
                            recipient.Id,
                            recipient.UniqueId,
                            ex.Message,
                            CommunicationChannel.Email));
                        _logger.LogError(
                            ex,
                            "Failed to send email to {Email} (AccountId: {AccountId})",
                            recipient.Email,
                            recipient.Id);
                    }
                }

                if (request.Channel == CommunicationChannel.Sms || request.Channel == CommunicationChannel.Both)
                {
                    try
                    {
                        await _notificationService.SendBulkSmsAsync(
                            recipient.MobileNumber,
                            request.Message,
                            cancellationToken);
                        smsSent++;
                    }
                    catch (Exception ex)
                    {
                        smsFailed++;
                        errors.Add(new BulkCommunicationError(
                            recipient.Id,
                            recipient.UniqueId,
                            ex.Message,
                            CommunicationChannel.Sms));
                        _logger.LogError(
                            ex,
                            "Failed to send SMS to {MobileNumber} (AccountId: {AccountId})",
                            recipient.MobileNumber,
                            recipient.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error processing recipient {AccountId}",
                    recipient.Id);
            }
        }

        _logger.LogInformation(
            "Bulk communication completed. Emails: {EmailsSent}/{Total} sent, {EmailsFailed} failed. SMS: {SmsSent}/{Total} sent, {SmsFailed} failed.",
            emailsSent,
            recipients.Count,
            emailsFailed,
            smsSent,
            recipients.Count,
            smsFailed);

        return new BulkCommunicationResult(
            recipients.Count,
            emailsSent,
            smsSent,
            emailsFailed,
            smsFailed,
            errors);
    }

    private async Task<List<RecipientInfo>> GetRecipientsAsync(
        BulkCommunicationRecipientFilter filter,
        CancellationToken cancellationToken)
    {
        var allAccounts = await _accountRepository.GetAllAsync(cancellationToken);
        var query = allAccounts.AsQueryable();

        // Filter by specific account IDs if provided
        if (filter.SpecificAccountIds != null && filter.SpecificAccountIds.Count > 0)
        {
            query = query.Where(a => filter.SpecificAccountIds.Contains(a.Id));
        }

        // Filter by status
        if (filter.Statuses != null && filter.Statuses.Count > 0)
        {
            query = query.Where(a => filter.Statuses.Contains(a.Status));
        }

        // Filter by application submitted status
        if (filter.IsApplicationSubmitted.HasValue)
        {
            query = query.Where(a => a.IsApplicationSubmitted == filter.IsApplicationSubmitted.Value);
        }

        // Filter by payment completed status
        if (filter.IsPaymentCompleted.HasValue)
        {
            query = query.Where(a => a.IsPaymentCompleted == filter.IsPaymentCompleted.Value);
        }

        // Filter by search term
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.Trim().ToLowerInvariant();
            query = query.Where(a =>
                a.FullName.ToLower().Contains(searchTerm) ||
                a.Email.ToLower().Contains(searchTerm) ||
                a.UniqueId.ToLower().Contains(searchTerm) ||
                a.MobileNumber.Contains(searchTerm));
        }

        var accounts = query.ToList();

        return accounts.Select(a => new RecipientInfo
        {
            Id = a.Id,
            UniqueId = a.UniqueId,
            FullName = a.FullName,
            Email = a.Email,
            MobileNumber = a.MobileNumber
        }).ToList();
    }

    private sealed class RecipientInfo
    {
        public Guid Id { get; set; }
        public string UniqueId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
    }
}









