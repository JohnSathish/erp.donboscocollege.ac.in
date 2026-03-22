using ERP.Domain.Students.Entities;

namespace ERP.Application.Students.Interfaces;

public interface IAcademicHistoryRepository
{
    Task<AcademicRecord> AddAcademicRecordAsync(AcademicRecord record, CancellationToken cancellationToken = default);

    Task<AcademicRecord?> GetAcademicRecordByIdAsync(Guid recordId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<AcademicRecord>> GetAcademicRecordsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);

    Task<CourseEnrollment> AddCourseEnrollmentAsync(CourseEnrollment enrollment, CancellationToken cancellationToken = default);

    Task<CourseEnrollment?> GetCourseEnrollmentByIdAsync(Guid enrollmentId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<CourseEnrollment>> GetCourseEnrollmentsByStudentIdAsync(Guid studentId, Guid? termId = null, CancellationToken cancellationToken = default);

    Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

