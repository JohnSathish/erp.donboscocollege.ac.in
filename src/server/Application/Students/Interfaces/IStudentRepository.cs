using ERP.Domain.Students.Entities;

namespace ERP.Application.Students.Interfaces;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Student?> GetByStudentNumberAsync(string studentNumber, CancellationToken cancellationToken = default);
    Task<Student?> GetByApplicantAccountIdAsync(Guid applicantAccountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Student>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Student> Students, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        bool? isActive = null,
        Guid? programId = null,
        string? academicYear = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    Task<Student> AddAsync(Student student, CancellationToken cancellationToken = default);
    Task UpdateAsync(Student student, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


