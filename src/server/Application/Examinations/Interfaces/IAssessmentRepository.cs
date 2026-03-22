using ERP.Domain.Examinations.Entities;

namespace ERP.Application.Examinations.Interfaces;

public interface IAssessmentRepository
{
    Task<Assessment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Assessment?> GetByIdWithComponentsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Assessment>> GetByCourseAndTermAsync(Guid courseId, Guid academicTermId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Assessment>> GetByClassSectionAsync(Guid classSectionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Assessment>> GetByAcademicTermAsync(Guid academicTermId, CancellationToken cancellationToken = default);
    Task<Assessment> AddAsync(Assessment assessment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Assessment assessment, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CodeExistsAsync(string code, Guid courseId, Guid academicTermId, Guid? excludeId = null, CancellationToken cancellationToken = default);
}





