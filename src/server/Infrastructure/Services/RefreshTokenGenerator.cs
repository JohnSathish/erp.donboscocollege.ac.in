using System.Security.Cryptography;
using System.Text;
using ERP.Application.Common.Interfaces;
using ERP.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace ERP.Infrastructure.Services;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly JwtSettings _settings;

    public RefreshTokenGenerator(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public RefreshTokenGenerationResult Generate()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(tokenBytes);
        var expires = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpiryDays);
        var tokenHash = Hash(token);

        return new RefreshTokenGenerationResult(token, tokenHash, expires);
    }

    public string Hash(string token)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}

