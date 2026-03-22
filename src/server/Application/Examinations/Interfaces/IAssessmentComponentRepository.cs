using ERP.Domain.Examinations.Entities;

namespace ERP.Application.Examinations.Interfaces;

public interface IAssessmentComponentRepository
{
    Task<AssessmentComponent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AssessmentComponent>> GetByAssessmentIdAsync(Guid assessmentId, CancellationToken cancellationToken = default);
    Task<AssessmentComponent> AddAsync(AssessmentComponent component, CancellationToken cancellationToken = default);
    Task UpdateAsync(AssessmentComponent component, CancellationToken cancellationToken = default);
    Task DeleteAsync(AssessmentComponent component, CancellationToken cancellationToken = default);
    Task DeleteByAssessmentIdAsync(Guid assessmentId, CancellationToken cancellationToken = default);
}





