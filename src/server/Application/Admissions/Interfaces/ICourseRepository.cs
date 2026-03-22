using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Course?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Course>> GetByProgramIdAsync(Guid programId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Course>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Course> Courses, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        bool? isActive = null,
        Guid? programId = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    Task<Course> AddAsync(Course course, CancellationToken cancellationToken = default);
    Task UpdateAsync(Course course, CancellationToken cancellationToken = default);
    Task DeleteAsync(Course course, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}









