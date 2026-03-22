using ERP.Application.Admissions.Commands.LoginStudentApplicant;
using ERP.Application.Admissions.Interfaces;
using ERP.Application.Common.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.ChangeStudentApplicantPassword;

public sealed class ChangeStudentApplicantPasswordCommandHandler : IRequestHandler<ChangeStudentApplicantPasswordCommand, LoginStudentApplicantResult>
{
    private readonly IApplicantAccountRepository _repository;
    private readonly IApplicantPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IApplicantNotificationService _notificationService;

    public ChangeStudentApplicantPasswordCommandHandler(
        IApplicantAccountRepository repository,
        IApplicantPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IApplicantNotificationService notificationService)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _notificationService = notificationService;
    }

    public async Task<LoginStudentApplicantResult> Handle(ChangeStudentApplicantPasswordCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account is null)
        {
            throw new UnauthorizedAccessException("Account not found.");
        }

        if (!_passwordHasher.VerifyPassword(account, account.PasswordHash, request.CurrentPassword))
        {
            throw new UnauthorizedAccessException("Current password is incorrect.");
        }

        var newHash = _passwordHasher.HashPassword(account, request.NewPassword);
        await _repository.UpdatePasswordAsync(account.Id, newHash, mustChangePassword: false, cancellationToken);
        await _repository.RevokeRefreshTokensByAccountAsync(account.Id, cancellationToken);

        account.MarkPasswordChanged();

        await _notificationService.SendPasswordChangeNotificationAsync(
            account.FullName,
            account.Email,
            cancellationToken);

        var jwtResult = _jwtTokenGenerator.GenerateToken(account);
        var refreshResult = _refreshTokenGenerator.Generate();

        var refreshToken = new ApplicantRefreshToken(account.Id, refreshResult.TokenHash, refreshResult.ExpiresAtUtc);
        await _repository.AddRefreshTokenAsync(refreshToken, cancellationToken);

        return new LoginStudentApplicantResult(
            jwtResult.Token,
            jwtResult.ExpiresAtUtc,
            refreshResult.Token,
            refreshResult.ExpiresAtUtc,
            account.UniqueId,
            account.Email,
            account.FullName,
            false);
    }
}

