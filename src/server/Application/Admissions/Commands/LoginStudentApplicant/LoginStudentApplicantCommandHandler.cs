using ERP.Application.Admissions.Interfaces;
using ERP.Application.Common.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.LoginStudentApplicant;

public sealed class LoginStudentApplicantCommandHandler : IRequestHandler<LoginStudentApplicantCommand, LoginStudentApplicantResult>
{
    private readonly IApplicantAccountRepository _repository;
    private readonly IApplicantPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IApplicantLoginRateLimiter _rateLimiter;

    public LoginStudentApplicantCommandHandler(
        IApplicantAccountRepository repository,
        IApplicantPasswordHasher passwordHasher,
        IJwtTokenGenerator tokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IApplicantLoginRateLimiter rateLimiter)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _rateLimiter = rateLimiter;
    }

    public async Task<LoginStudentApplicantResult> Handle(LoginStudentApplicantCommand request, CancellationToken cancellationToken)
    {
        var rateLimitKey = request.Username.ToLowerInvariant();

        if (await _rateLimiter.IsLockedOutAsync(rateLimitKey, cancellationToken))
        {
            throw new UnauthorizedAccessException("Too many login attempts. Please try again later.");
        }

        var account = await FindAccountAsync(request.Username, cancellationToken);
        if (account is null)
        {
            await _rateLimiter.RegisterFailedAttemptAsync(rateLimitKey, cancellationToken);
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        if (!_passwordHasher.VerifyPassword(account, account.PasswordHash, request.Password))
        {
            await _rateLimiter.RegisterFailedAttemptAsync(rateLimitKey, cancellationToken);
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        await _rateLimiter.ResetAttemptsAsync(rateLimitKey, cancellationToken);

        var tokenResult = _tokenGenerator.GenerateToken(account);
        var refreshTokenResult = _refreshTokenGenerator.Generate();

        var refreshToken = new ApplicantRefreshToken(account.Id, refreshTokenResult.TokenHash, refreshTokenResult.ExpiresAtUtc);
        await _repository.AddRefreshTokenAsync(refreshToken, cancellationToken);

        return new LoginStudentApplicantResult(
            tokenResult.Token,
            tokenResult.ExpiresAtUtc,
            refreshTokenResult.Token,
            refreshTokenResult.ExpiresAtUtc,
            account.UniqueId,
            account.Email,
            account.FullName,
            account.MustChangePassword);
    }

    private async Task<StudentApplicantAccount?> FindAccountAsync(string username, CancellationToken cancellationToken)
    {
        if (username.Contains('@', StringComparison.Ordinal))
        {
            return await _repository.GetByEmailAsync(username, cancellationToken);
        }

        return await _repository.GetByUniqueIdAsync(username, cancellationToken);
    }
}

