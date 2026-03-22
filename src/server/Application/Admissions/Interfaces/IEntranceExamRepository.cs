using ERP.Domain.Admissions.Entities;

namespace ERP.Application.Admissions.Interfaces;

public interface IEntranceExamRepository
{
    Task<EntranceExam?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<EntranceExam?> GetByCodeAsync(string examCode, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<EntranceExam>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<EntranceExam>> GetActiveExamsAsync(CancellationToken cancellationToken = default);

    Task<(IReadOnlyCollection<EntranceExam> Exams, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        bool? isActive = null,
        DateTime? examDateFrom = null,
        DateTime? examDateTo = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(EntranceExam exam, CancellationToken cancellationToken = default);

    void Update(EntranceExam exam);

    Task UpdateAsync(EntranceExam exam, CancellationToken cancellationToken = default);

    Task DeleteAsync(EntranceExam exam, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

