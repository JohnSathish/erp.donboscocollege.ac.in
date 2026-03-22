using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using Microsoft.AspNetCore.Identity;

namespace ERP.Infrastructure.Services;

public class AdminPasswordHasher : IAdminPasswordHasher
{
    private readonly IPasswordHasher<AdminUser> _passwordHasher;

    public AdminPasswordHasher(IPasswordHasher<AdminUser> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(AdminUser adminUser, string password)
    {
        return _passwordHasher.HashPassword(adminUser, password);
    }

    public bool VerifyPassword(AdminUser adminUser, string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(adminUser, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}














