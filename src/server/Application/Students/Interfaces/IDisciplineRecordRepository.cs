using ERP.Domain.Students.Entities;

namespace ERP.Application.Students.Interfaces;

public interface IDisciplineRecordRepository
{
    Task<DisciplineRecord> AddAsync(DisciplineRecord record, CancellationToken cancellationToken = default);
    Task<DisciplineRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<DisciplineRecord>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<DisciplineRecord>> GetUnresolvedRecordsAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(DisciplineRecord record, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

