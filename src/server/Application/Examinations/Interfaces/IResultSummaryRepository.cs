using ERP.Domain.Examinations.Entities;

namespace ERP.Application.Examinations.Interfaces;

public interface IResultSummaryRepository
{
    Task<ResultSummary?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultSummary?> GetByIdWithCourseResultsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultSummary?> GetByStudentAndTermAsync(Guid studentId, Guid academicTermId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ResultSummary>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ResultSummary>> GetByAcademicTermIdAsync(Guid academicTermId, CancellationToken cancellationToken = default);
    Task<ResultSummary> AddAsync(ResultSummary resultSummary, CancellationToken cancellationToken = default);
    Task UpdateAsync(ResultSummary resultSummary, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid studentId, Guid academicTermId, CancellationToken cancellationToken = default);
}





