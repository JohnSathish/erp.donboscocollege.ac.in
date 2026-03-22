using ERP.Application.Admissions.Interfaces;
using ERP.Application.Common.Interfaces;
using ERP.Domain.Admissions.Entities;
using MediatR;

namespace ERP.Application.Admissions.Commands.LoginAdmin;

public sealed class LoginAdminCommandHandler : IRequestHandler<LoginAdminCommand, LoginAdminResult>
{
    private readonly IAdminUserRepository _repository;
    private readonly IAdminPasswordHasher _passwordHasher;
    private readonly IAdminJwtTokenGenerator _tokenGenerator;

    public LoginAdminCommandHandler(
        IAdminUserRepository repository,
        IAdminPasswordHasher passwordHasher,
        IAdminJwtTokenGenerator tokenGenerator)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<LoginAdminResult> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
    {
        var adminUser = await FindAdminUserAsync(request.Username, cancellationToken);
        if (adminUser is null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        if (!_passwordHasher.VerifyPassword(adminUser, adminUser.PasswordHash, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        adminUser.UpdateLastLogin();
        await _repository.UpdateAsync(adminUser, cancellationToken);

        var tokenResult = _tokenGenerator.GenerateToken(adminUser);

        return new LoginAdminResult(
            tokenResult.Token,
            tokenResult.ExpiresAtUtc,
            adminUser.Id.ToString(),
            adminUser.Email,
            adminUser.FullName);
    }

    private async Task<AdminUser?> FindAdminUserAsync(string username, CancellationToken cancellationToken)
    {
        if (username.Contains('@', StringComparison.Ordinal))
        {
            return await _repository.GetByEmailAsync(username, cancellationToken);
        }

        return await _repository.GetByUsernameAsync(username, cancellationToken);
    }
}

