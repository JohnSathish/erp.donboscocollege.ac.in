using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    JwtTokenResult GenerateToken(StudentApplicantAccount account, TimeSpan? lifetime = null);
}

public sealed record JwtTokenResult(string Token, DateTime ExpiresAtUtc);

