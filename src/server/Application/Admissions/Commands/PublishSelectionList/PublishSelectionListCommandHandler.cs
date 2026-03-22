using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.PublishSelectionList;

public sealed class PublishSelectionListCommandHandler
    : IRequestHandler<PublishSelectionListCommand, PublishSelectionListResult>
{
    private readonly IApplicantAccountRepository _repository;
    private readonly IApplicantNotificationService _notificationService;
    private readonly ILogger<PublishSelectionListCommandHandler> _logger;

    public PublishSelectionListCommandHandler(
        IApplicantAccountRepository repository,
        IApplicantNotificationService notificationService,
        ILogger<PublishSelectionListCommandHandler> logger)
    {
        _repository = repository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<PublishSelectionListResult> Handle(
        PublishSelectionListCommand request,
        CancellationToken cancellationToken)
    {
        var accounts = (await _repository.GetAccountsForSelectionListPublishAsync(
                request.Round,
                cancellationToken))
            .ToList();

        if (accounts.Count == 0)
        {
            return new PublishSelectionListResult(0, 0, 0);
        }

        var utc = DateTime.UtcNow;
        foreach (var account in accounts)
        {
            account.MarkSelectionListPublished(utc);
            await _repository.UpdateAsync(account, cancellationToken);
        }

        var smsSent = 0;
        var smsFailed = 0;
        if (request.SendSmsNotifications)
        {
            var roundLabel = request.Round.ToString();
            foreach (var account in accounts)
            {
                var msg =
                    $"Congratulations! Your application {account.UniqueId} is listed on the {roundLabel} selection list. — Don Bosco College, Tura";
                try
                {
                    await _notificationService.SendBulkSmsAsync(
                        account.MobileNumber,
                        msg,
                        cancellationToken);
                    smsSent++;
                }
                catch (Exception ex)
                {
                    smsFailed++;
                    _logger.LogError(
                        ex,
                        "SMS failed for selection list publish. Account {Id}",
                        account.Id);
                }
            }
        }

        return new PublishSelectionListResult(accounts.Count, smsSent, smsFailed);
    }
}
