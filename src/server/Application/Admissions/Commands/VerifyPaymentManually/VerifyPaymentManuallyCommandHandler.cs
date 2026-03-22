using ERP.Application.Admissions.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Admissions.Commands.VerifyPaymentManually;

public sealed class VerifyPaymentManuallyCommandHandler : IRequestHandler<VerifyPaymentManuallyCommand, VerifyPaymentManuallyResult>
{
    private readonly IApplicantAccountRepository _accountRepository;
    private readonly ILogger<VerifyPaymentManuallyCommandHandler> _logger;

    public VerifyPaymentManuallyCommandHandler(
        IApplicantAccountRepository accountRepository,
        ILogger<VerifyPaymentManuallyCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<VerifyPaymentManuallyResult> Handle(VerifyPaymentManuallyCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdForUpdateAsync(request.AccountId, cancellationToken);

        if (account is null)
        {
            throw new InvalidOperationException($"Account with ID {request.AccountId} not found.");
        }

        if (account.IsPaymentCompleted)
        {
            throw new InvalidOperationException($"Payment for application {account.UniqueId} has already been completed.");
        }

        // Mark payment as completed
        account.MarkPaymentCompleted(request.TransactionId, request.Amount);

        await _accountRepository.UpdateAsync(account, cancellationToken);

        _logger.LogInformation(
            "Payment manually verified for application {UniqueId} by {VerifiedBy}. Transaction ID: {TransactionId}, Amount: {Amount}",
            account.UniqueId,
            request.VerifiedBy ?? "Unknown",
            request.TransactionId,
            request.Amount);

        return new VerifyPaymentManuallyResult(request.AccountId);
    }
}











