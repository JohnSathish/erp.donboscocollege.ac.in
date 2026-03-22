using ERP.Domain.Students.Entities;

namespace ERP.Application.Students.Interfaces;

public interface ICounselingRecordRepository
{
    Task<CounselingRecord> AddAsync(CounselingRecord record, CancellationToken cancellationToken = default);
    Task<CounselingRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CounselingRecord>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CounselingRecord>> GetUpcomingSessionsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CounselingRecord>> GetSessionsRequiringFollowUpAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(CounselingRecord record, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

