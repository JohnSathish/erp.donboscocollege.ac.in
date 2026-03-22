using ERP.Application.Admissions.Commands.LoginStudentApplicant;
using ERP.Application.Admissions.Interfaces;
using ERP.Application.Common.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.RefreshStudentApplicantToken;

public sealed class RefreshStudentApplicantTokenCommandHandler : IRequestHandler<RefreshStudentApplicantTokenCommand, LoginStudentApplicantResult>
{
    private readonly IApplicantAccountRepository _repository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public RefreshStudentApplicantTokenCommandHandler(
        IApplicantAccountRepository repository,
        IJwtTokenGenerator tokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _repository = repository;
        _tokenGenerator = tokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<LoginStudentApplicantResult> Handle(RefreshStudentApplicantTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = _refreshTokenGenerator.Hash(request.RefreshToken);
        var storedToken = await _repository.GetRefreshTokenAsync(tokenHash, cancellationToken);

        if (storedToken is null || storedToken.IsRevoked || storedToken.IsExpired())
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        var account = storedToken.Account;
        if (account is null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        await _repository.RevokeRefreshTokenAsync(storedToken, cancellationToken);

        var jwtResult = _tokenGenerator.GenerateToken(account);
        var refreshResult = _refreshTokenGenerator.Generate();

        var newRefreshToken = new ApplicantRefreshToken(account.Id, refreshResult.TokenHash, refreshResult.ExpiresAtUtc);
        await _repository.AddRefreshTokenAsync(newRefreshToken, cancellationToken);

        return new LoginStudentApplicantResult(
            jwtResult.Token,
            jwtResult.ExpiresAtUtc,
            refreshResult.Token,
            refreshResult.ExpiresAtUtc,
            account.UniqueId,
            account.Email,
            account.FullName,
            account.MustChangePassword);
    }
}

