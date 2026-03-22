using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public class AdminUserRepository : IAdminUserRepository
{
    private readonly ApplicationDbContext _context;

    public AdminUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.AdminUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username && x.IsActive, cancellationToken);
    }

    public async Task<AdminUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.AdminUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email && x.IsActive, cancellationToken);
    }

    public async Task<AdminUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AdminUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);
    }

    public async Task UpdateAsync(AdminUser adminUser, CancellationToken cancellationToken = default)
    {
        _context.AdminUsers.Update(adminUser);
        await _context.SaveChangesAsync(cancellationToken);
    }
}














