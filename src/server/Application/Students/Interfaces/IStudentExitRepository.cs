using ERP.Domain.Students.Entities;

namespace ERP.Application.Students.Interfaces;

public interface IStudentExitRepository
{
    Task<StudentExit> AddAsync(StudentExit exit, CancellationToken cancellationToken = default);
    Task<StudentExit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<StudentExit?> GetActiveExitByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<StudentExit>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<StudentExit>> GetPendingExitsAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(StudentExit exit, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

