using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IAdminUserRepository
{
    Task<AdminUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<AdminUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<AdminUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpdateAsync(AdminUser adminUser, CancellationToken cancellationToken = default);
}














