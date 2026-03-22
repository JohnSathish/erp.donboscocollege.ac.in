namespace ERP.Application.Admissions.Interfaces;

public interface IApplicantLoginRateLimiter
{
    Task<bool> IsLockedOutAsync(string key, CancellationToken cancellationToken = default);

    Task RegisterFailedAttemptAsync(string key, CancellationToken cancellationToken = default);

    Task ResetAttemptsAsync(string key, CancellationToken cancellationToken = default);
}

