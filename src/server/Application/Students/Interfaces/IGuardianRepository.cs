using ERP.Domain.Students.Entities;

namespace ERP.Application.Students.Interfaces;

public interface IGuardianRepository
{
    Task<Guardian> AddAsync(Guardian guardian, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Guardian>> AddRangeAsync(IReadOnlyCollection<Guardian> guardians, CancellationToken cancellationToken = default);

    Task<Guardian?> GetByIdAsync(Guid guardianId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Guardian>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guardian guardian, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

