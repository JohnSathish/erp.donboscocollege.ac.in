namespace ERP.Infrastructure.Authentication;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;

    public string Issuer { get; set; } = "ERP.Api";

    public string Audience { get; set; } = "ERP.Applicants";

    public int ExpiryMinutes { get; set; } = 60;

    public int RefreshTokenExpiryDays { get; set; } = 30;
}

