using ERP.Domain.Examinations.Entities;

namespace ERP.Application.Examinations.Interfaces;

public interface IMarkEntryRepository
{
    Task<MarkEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MarkEntry?> GetByComponentAndStudentAsync(Guid assessmentComponentId, Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<MarkEntry>> GetByAssessmentComponentIdAsync(Guid assessmentComponentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<MarkEntry>> GetByStudentIdAsync(Guid studentId, Guid? academicTermId = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<MarkEntry>> GetByAssessmentIdAsync(Guid assessmentId, CancellationToken cancellationToken = default);
    Task<MarkEntry> AddAsync(MarkEntry markEntry, CancellationToken cancellationToken = default);
    Task UpdateAsync(MarkEntry markEntry, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<MarkEntry> markEntries, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<MarkEntry> markEntries, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid assessmentComponentId, Guid studentId, CancellationToken cancellationToken = default);
}





