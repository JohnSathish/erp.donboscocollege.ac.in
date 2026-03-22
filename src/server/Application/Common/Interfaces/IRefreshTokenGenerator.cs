namespace ERP.Application.Common.Interfaces;

public interface IRefreshTokenGenerator
{
    RefreshTokenGenerationResult Generate();

    string Hash(string token);
}

public sealed record RefreshTokenGenerationResult(string Token, string TokenHash, DateTime ExpiresAtUtc);

