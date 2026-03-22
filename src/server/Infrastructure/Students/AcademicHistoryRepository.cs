using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Students;

public class AcademicHistoryRepository : IAcademicHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public AcademicHistoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AcademicRecord> AddAcademicRecordAsync(AcademicRecord record, CancellationToken cancellationToken = default)
    {
        var entry = await _context.AcademicRecords.AddAsync(record, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<AcademicRecord?> GetAcademicRecordByIdAsync(Guid recordId, CancellationToken cancellationToken = default)
    {
        return await _context.AcademicRecords
            .FirstOrDefaultAsync(r => r.Id == recordId, cancellationToken);
    }

    public Task<IReadOnlyCollection<AcademicRecord>> GetAcademicRecordsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return _context.AcademicRecords
            .AsNoTracking()
            .Where(r => r.StudentId == studentId)
            .OrderByDescending(r => r.AcademicYear)
            .ThenByDescending(r => r.Semester)
            .ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyCollection<AcademicRecord>)t.Result);
    }

    public async Task<CourseEnrollment> AddCourseEnrollmentAsync(CourseEnrollment enrollment, CancellationToken cancellationToken = default)
    {
        var entry = await _context.CourseEnrollments.AddAsync(enrollment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<CourseEnrollment?> GetCourseEnrollmentByIdAsync(Guid enrollmentId, CancellationToken cancellationToken = default)
    {
        return await _context.CourseEnrollments
            .FirstOrDefaultAsync(e => e.Id == enrollmentId, cancellationToken);
    }

    public Task<IReadOnlyCollection<CourseEnrollment>> GetCourseEnrollmentsByStudentIdAsync(Guid studentId, Guid? termId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.CourseEnrollments
            .AsNoTracking()
            .Where(e => e.StudentId == studentId);

        if (termId.HasValue)
        {
            query = query.Where(e => e.TermId == termId.Value);
        }

        return query
            .OrderBy(e => e.EnrolledOnUtc)
            .ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyCollection<CourseEnrollment>)t.Result);
    }

    public async Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
    {
        _context.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}

