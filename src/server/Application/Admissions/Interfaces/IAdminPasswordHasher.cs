using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IAdminPasswordHasher
{
    string HashPassword(AdminUser adminUser, string password);

    bool VerifyPassword(AdminUser adminUser, string hashedPassword, string providedPassword);
}














