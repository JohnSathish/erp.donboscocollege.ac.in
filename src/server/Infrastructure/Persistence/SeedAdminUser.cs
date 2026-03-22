using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Persistence;

/// <summary>
/// Seeds a default admin user. Run this after applying the AddAdminUsers migration.
/// Default credentials: username: admin, password: Admin@123
/// </summary>
public static class SeedAdminUser
{
    public static async Task SeedOrUpdateAsync(ApplicationDbContext context, IPasswordHasher<AdminUser> passwordHasher)
    {
        var existingAdmin = await context.AdminUsers
            .FirstOrDefaultAsync(x => x.Username == "admin");

        const string defaultPassword = "Admin@123";

        if (existingAdmin != null)
        {
            // Check if password hash is valid by trying to verify it
            try
            {
                var result = passwordHasher.VerifyHashedPassword(existingAdmin, existingAdmin.PasswordHash, defaultPassword);
                if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    // Password hash is valid, no need to update
                    return;
                }
            }
            catch
            {
                // Password hash is invalid, update it
            }

            // Update existing admin with valid password hash
            var newPasswordHash = passwordHasher.HashPassword(existingAdmin, defaultPassword);
            existingAdmin.SetPasswordHash(newPasswordHash);
            context.AdminUsers.Update(existingAdmin);
            await context.SaveChangesAsync();
            return;
        }

        // Create new admin user
        var adminUser = new AdminUser(
            username: "admin",
            email: "admin@donboscocollege.ac.in",
            fullName: "System Administrator",
            passwordHash: string.Empty);

        var passwordHash = passwordHasher.HashPassword(adminUser, defaultPassword);
        adminUser.SetPasswordHash(passwordHash);

        await context.AdminUsers.AddAsync(adminUser);
        await context.SaveChangesAsync();
    }
}

