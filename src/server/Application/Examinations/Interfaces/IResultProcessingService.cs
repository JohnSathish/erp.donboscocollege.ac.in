using ERP.Domain.Examinations.Entities;

namespace ERP.Application.Examinations.Interfaces;

public interface IResultProcessingService
{
    Task<ResultSummary> ProcessStudentResultsAsync(
        Guid studentId,
        Guid academicTermId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ResultSummary>> ProcessTermResultsAsync(
        Guid academicTermId,
        CancellationToken cancellationToken = default);

    Task<CourseResult> CalculateCourseResultAsync(
        Guid studentId,
        Guid courseId,
        Guid academicTermId,
        CancellationToken cancellationToken = default);
}





