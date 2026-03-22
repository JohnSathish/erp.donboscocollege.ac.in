namespace ERP.Domain.Admissions.Entities;

public class ApplicantRefreshToken
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid AccountId { get; private set; }

    public string TokenHash { get; private set; }

    public DateTime ExpiresOnUtc { get; private set; }

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public DateTime? RevokedOnUtc { get; private set; }

    public bool IsRevoked => RevokedOnUtc.HasValue;

    public StudentApplicantAccount? Account { get; private set; }

    private ApplicantRefreshToken()
    {
        TokenHash = string.Empty;
    }

    public ApplicantRefreshToken(Guid accountId, string tokenHash, DateTime expiresOnUtc)
    {
        AccountId = accountId;
        TokenHash = tokenHash;
        ExpiresOnUtc = expiresOnUtc;
    }

    public bool IsExpired() => DateTime.UtcNow >= ExpiresOnUtc;

    public void Revoke()
    {
        RevokedOnUtc = DateTime.UtcNow;
    }
}

