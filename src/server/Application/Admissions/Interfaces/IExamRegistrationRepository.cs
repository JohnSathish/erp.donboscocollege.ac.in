using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IExamRegistrationRepository
{
    Task<ExamRegistration?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ExamRegistration?> GetByExamAndApplicantAsync(
        Guid examId,
        Guid applicantAccountId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ExamRegistration>> GetByExamIdAsync(
        Guid examId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ExamRegistration>> GetByApplicantIdAsync(
        Guid applicantAccountId,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyCollection<ExamRegistration> Registrations, int TotalCount)> GetPagedByExamAsync(
        Guid examId,
        int page,
        int pageSize,
        bool? isPresent = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(ExamRegistration registration, CancellationToken cancellationToken = default);

    void Update(ExamRegistration registration);

    Task UpdateAsync(ExamRegistration registration, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

