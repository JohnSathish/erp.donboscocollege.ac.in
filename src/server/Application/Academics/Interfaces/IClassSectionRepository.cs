using ERP.Domain.Academics.Entities;

namespace ERP.Application.Academics.Interfaces;

public interface IClassSectionRepository
{
    Task<ClassSection?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ClassSection?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ClassSection>> GetByCourseIdAsync(Guid courseId, string? academicYear = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ClassSection>> GetByTeacherIdAsync(Guid teacherId, string? academicYear = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ClassSection>> GetByAcademicYearAsync(string academicYear, string? shift = null, CancellationToken cancellationToken = default);
    Task<ClassSection> AddAsync(ClassSection section, CancellationToken cancellationToken = default);
    Task UpdateAsync(ClassSection section, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

