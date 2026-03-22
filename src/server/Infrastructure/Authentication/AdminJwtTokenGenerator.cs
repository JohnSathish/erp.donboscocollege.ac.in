using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ERP.Application.Common.Interfaces;
using ERP.Domain.Admissions.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ERP.Infrastructure.Authentication;

public class AdminJwtTokenGenerator : IAdminJwtTokenGenerator
{
    private readonly JwtSettings _settings;

    public AdminJwtTokenGenerator(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public JwtTokenResult GenerateToken(AdminUser adminUser, TimeSpan? lifetime = null)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, adminUser.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, adminUser.Email),
            new("username", adminUser.Username),
            new("name", adminUser.FullName),
            new(ClaimTypes.Role, "Admin")
        };

        var expires = DateTime.UtcNow.Add(lifetime ?? TimeSpan.FromMinutes(_settings.ExpiryMinutes));

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return new JwtTokenResult(tokenString, expires);
    }
}














