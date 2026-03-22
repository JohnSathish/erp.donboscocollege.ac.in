using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public sealed class EntranceExamRepository(ApplicationDbContext context) : IEntranceExamRepository
{
    public async Task<EntranceExam?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.EntranceExams
            .Include(x => x.Registrations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<EntranceExam?> GetByCodeAsync(string examCode, CancellationToken cancellationToken = default)
    {
        return await context.EntranceExams
            .Include(x => x.Registrations)
            .FirstOrDefaultAsync(x => x.ExamCode == examCode, cancellationToken);
    }

    public async Task<IReadOnlyCollection<EntranceExam>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.EntranceExams
            .OrderByDescending(x => x.ExamDate)
            .ThenByDescending(x => x.CreatedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<EntranceExam>> GetActiveExamsAsync(CancellationToken cancellationToken = default)
    {
        return await context.EntranceExams
            .Where(x => x.IsActive && x.ExamDate >= DateTime.UtcNow.Date)
            .OrderBy(x => x.ExamDate)
            .ThenBy(x => x.ExamStartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyCollection<EntranceExam> Exams, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        bool? isActive = null,
        DateTime? examDateFrom = null,
        DateTime? examDateTo = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.EntranceExams.AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        if (examDateFrom.HasValue)
        {
            query = query.Where(x => x.ExamDate >= examDateFrom.Value);
        }

        if (examDateTo.HasValue)
        {
            query = query.Where(x => x.ExamDate <= examDateTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLowerInvariant();
            query = query.Where(x =>
                x.ExamName.ToLower().Contains(term) ||
                x.ExamCode.ToLower().Contains(term) ||
                (x.Venue != null && x.Venue.ToLower().Contains(term)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var exams = await query
            .OrderByDescending(x => x.ExamDate)
            .ThenByDescending(x => x.CreatedOnUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (exams, totalCount);
    }

    public async Task AddAsync(EntranceExam exam, CancellationToken cancellationToken = default)
    {
        await context.EntranceExams.AddAsync(exam, cancellationToken);
    }

    public void Update(EntranceExam exam)
    {
        context.EntranceExams.Update(exam);
    }

    public async Task UpdateAsync(EntranceExam exam, CancellationToken cancellationToken = default)
    {
        context.EntranceExams.Update(exam);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(EntranceExam exam, CancellationToken cancellationToken = default)
    {
        context.EntranceExams.Remove(exam);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}

