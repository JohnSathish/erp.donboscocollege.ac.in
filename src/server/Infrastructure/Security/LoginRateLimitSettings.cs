namespace ERP.Infrastructure.Security;

public class LoginRateLimitSettings
{
    public int MaxAttempts { get; set; } = 5;

    public TimeSpan AttemptWindow { get; set; } = TimeSpan.FromMinutes(5);

    public TimeSpan LockoutDuration { get; set; } = TimeSpan.FromMinutes(15);
}

