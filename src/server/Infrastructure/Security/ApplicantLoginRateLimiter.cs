using System.Collections.Concurrent;
using ERP.Application.Admissions.Interfaces;
using Microsoft.Extensions.Options;

namespace ERP.Infrastructure.Security;

public class ApplicantLoginRateLimiter : IApplicantLoginRateLimiter
{
    private readonly ConcurrentDictionary<string, AttemptWindow> _attempts = new();
    private readonly LoginRateLimitSettings _settings;

    public ApplicantLoginRateLimiter(IOptions<LoginRateLimitSettings> options)
    {
        _settings = options.Value;
    }

    public Task<bool> IsLockedOutAsync(string key, CancellationToken cancellationToken = default)
    {
        var window = _attempts.GetOrAdd(key, _ => new AttemptWindow());
        window.TrimExpired(_settings);
        return Task.FromResult(window.IsLocked(_settings));
    }

    public Task RegisterFailedAttemptAsync(string key, CancellationToken cancellationToken = default)
    {
        var window = _attempts.GetOrAdd(key, _ => new AttemptWindow());
        window.AddAttempt(_settings);
        return Task.CompletedTask;
    }

    public Task ResetAttemptsAsync(string key, CancellationToken cancellationToken = default)
    {
        _attempts.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    private sealed class AttemptWindow
    {
        private readonly List<DateTime> _attempts = new();
        private DateTime? _lockedUntilUtc;

        public void AddAttempt(LoginRateLimitSettings settings)
        {
            var now = DateTime.UtcNow;
            _attempts.Add(now);
            TrimExpired(settings);

            if (_attempts.Count >= settings.MaxAttempts)
            {
                _lockedUntilUtc = now.Add(settings.LockoutDuration);
                _attempts.Clear();
            }
        }

        public void TrimExpired(LoginRateLimitSettings settings)
        {
            var cutoff = DateTime.UtcNow - settings.AttemptWindow;
            _attempts.RemoveAll(attempt => attempt < cutoff);

            if (_lockedUntilUtc.HasValue && DateTime.UtcNow >= _lockedUntilUtc)
            {
                _lockedUntilUtc = null;
            }
        }

        public bool IsLocked(LoginRateLimitSettings settings)
        {
            TrimExpired(settings);
            return _lockedUntilUtc.HasValue;
        }
    }
}

