using ERP.Application.Admissions.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public sealed class ExamRegistrationRepository(ApplicationDbContext context) : IExamRegistrationRepository
{
    public async Task<ExamRegistration?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ExamRegistrations
            .Include(x => x.Exam)
            .Include(x => x.ApplicantAccount)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ExamRegistration?> GetByExamAndApplicantAsync(
        Guid examId,
        Guid applicantAccountId,
        CancellationToken cancellationToken = default)
    {
        return await context.ExamRegistrations
            .Include(x => x.Exam)
            .Include(x => x.ApplicantAccount)
            .FirstOrDefaultAsync(
                x => x.ExamId == examId && x.ApplicantAccountId == applicantAccountId,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<ExamRegistration>> GetByExamIdAsync(
        Guid examId,
        CancellationToken cancellationToken = default)
    {
        return await context.ExamRegistrations
            .Include(x => x.ApplicantAccount)
            .Where(x => x.ExamId == examId)
            .OrderBy(x => x.ApplicantAccount.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ExamRegistration>> GetByApplicantIdAsync(
        Guid applicantAccountId,
        CancellationToken cancellationToken = default)
    {
        return await context.ExamRegistrations
            .Include(x => x.Exam)
            .Where(x => x.ApplicantAccountId == applicantAccountId)
            .OrderByDescending(x => x.Exam.ExamDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyCollection<ExamRegistration> Registrations, int TotalCount)> GetPagedByExamAsync(
        Guid examId,
        int page,
        int pageSize,
        bool? isPresent = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.ExamRegistrations
            .Include(x => x.Exam)
            .Include(x => x.ApplicantAccount)
            .Where(x => x.ExamId == examId)
            .AsQueryable();

        if (isPresent.HasValue)
        {
            query = query.Where(x => x.IsPresent == isPresent.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLowerInvariant();
            query = query.Where(x =>
                (x.ApplicantAccount != null && x.ApplicantAccount.FullName.ToLower().Contains(term)) ||
                (x.ApplicantAccount != null && x.ApplicantAccount.UniqueId.ToLower().Contains(term)) ||
                (x.ApplicantAccount != null && x.ApplicantAccount.Email.ToLower().Contains(term)) ||
                (x.HallTicketNumber != null && x.HallTicketNumber.ToLower().Contains(term)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var registrations = await query
            .OrderBy(x => x.ApplicantAccount != null ? x.ApplicantAccount.FullName : string.Empty)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (registrations, totalCount);
    }

    public async Task AddAsync(ExamRegistration registration, CancellationToken cancellationToken = default)
    {
        await context.ExamRegistrations.AddAsync(registration, cancellationToken);
    }

    public void Update(ExamRegistration registration)
    {
        context.ExamRegistrations.Update(registration);
    }

    public async Task UpdateAsync(ExamRegistration registration, CancellationToken cancellationToken = default)
    {
        context.ExamRegistrations.Update(registration);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}

