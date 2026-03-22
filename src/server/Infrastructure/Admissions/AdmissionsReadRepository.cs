using System.Linq;
using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.ViewModels;
using ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Admissions;

public class AdmissionsReadRepository : IAdmissionsReadRepository
{
    private readonly ApplicationDbContext _context;

    public AdmissionsReadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicantDto?> GetApplicantByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Applicants
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ApplicantDto(
                x.Id,
                x.ApplicationNumber,
                BuildFullName(x.FirstName, x.LastName),
                x.Email,
                x.MobileNumber,
                x.DateOfBirth,
                x.ProgramCode,
                x.Status.ToString(),
                x.StatusUpdatedOnUtc,
                x.StatusUpdatedBy,
                x.StatusRemarks,
                x.EntranceExamScheduledOnUtc,
                x.EntranceExamVenue,
                x.EntranceExamInstructions,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ApplicantDto>> ListApplicantsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Applicants
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedOnUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ApplicantDto(
                x.Id,
                x.ApplicationNumber,
                BuildFullName(x.FirstName, x.LastName),
                x.Email,
                x.MobileNumber,
                x.DateOfBirth,
                x.ProgramCode,
                x.Status.ToString(),
                x.StatusUpdatedOnUtc,
                x.StatusUpdatedBy,
                x.StatusRemarks,
                x.EntranceExamScheduledOnUtc,
                x.EntranceExamVenue,
                x.EntranceExamInstructions,
                x.CreatedOnUtc))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ApplicantDto>> ListAllApplicantsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Applicants
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedOnUtc)
            .Select(x => new ApplicantDto(
                x.Id,
                x.ApplicationNumber,
                BuildFullName(x.FirstName, x.LastName),
                x.Email,
                x.MobileNumber,
                x.DateOfBirth,
                x.ProgramCode,
                x.Status.ToString(),
                x.StatusUpdatedOnUtc,
                x.StatusUpdatedBy,
                x.StatusRemarks,
                x.EntranceExamScheduledOnUtc,
                x.EntranceExamVenue,
                x.EntranceExamInstructions,
                x.CreatedOnUtc))
            .ToListAsync(cancellationToken);
    }

    private static string BuildFullName(string firstName, string lastName)
    {
        return string.Join(' ', new[] { firstName, lastName }
            .Where(v => !string.IsNullOrWhiteSpace(v)))
            .Trim();
    }
}


