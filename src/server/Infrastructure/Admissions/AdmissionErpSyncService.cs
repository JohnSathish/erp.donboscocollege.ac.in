using System.Text.Json;
using ERP.Application.Admissions.DTOs;
using ERP.Application.Admissions.Interfaces;
using ERP.Application.Admissions.Options;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Admissions.Entities;
using ERP.Domain.Students.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ERP.Infrastructure.Admissions;

public sealed class AdmissionErpSyncService : IAdmissionErpSyncService
{
    private readonly IApplicantAccountRepository _accounts;
    private readonly IApplicantApplicationRepository _applications;
    private readonly IStudentRepository _students;
    private readonly IOptions<AdmissionErpSyncOptions> _options;
    private readonly ILogger<AdmissionErpSyncService> _logger;
    private readonly JsonSerializerOptions _json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
    };

    public AdmissionErpSyncService(
        IApplicantAccountRepository accounts,
        IApplicantApplicationRepository applications,
        IStudentRepository students,
        IOptions<AdmissionErpSyncOptions> options,
        ILogger<AdmissionErpSyncService> logger)
    {
        _accounts = accounts;
        _applications = applications;
        _students = students;
        _options = options;
        _logger = logger;
    }

    public async Task<AdmissionErpSyncResult> TrySyncApprovedApplicationAsync(
        Guid applicantAccountId,
        CancellationToken cancellationToken = default)
    {
        var opt = _options.Value;
        if (!opt.Enabled)
        {
            return new AdmissionErpSyncResult(false, null, "ERP sync is disabled (AdmissionErpSync:Enabled=false).");
        }

        if (string.IsNullOrWhiteSpace(opt.AcademicYear))
        {
            return new AdmissionErpSyncResult(false, null, "AdmissionErpSync:AcademicYear is not configured.");
        }

        var account = await _accounts.GetByIdForUpdateAsync(applicantAccountId, cancellationToken);
        if (account is null)
        {
            return new AdmissionErpSyncResult(false, null, "Applicant account not found.");
        }

        if (account.Status != ApplicationStatus.Approved)
        {
            return new AdmissionErpSyncResult(false, null, $"Application must be Approved (current: {account.Status}).");
        }

        if (account.ErpStudentId is { } existingId)
        {
            return new AdmissionErpSyncResult(true, existingId, "Already linked to ERP student.");
        }

        var existingStudent = await _students.GetByApplicantAccountIdAsync(applicantAccountId, cancellationToken);
        if (existingStudent is not null)
        {
            account.RecordErpStudentLink(existingStudent.Id, DateTime.UtcNow);
            await _accounts.UpdateAsync(account, cancellationToken);
            return new AdmissionErpSyncResult(true, existingStudent.Id, "Linked to existing ERP student.");
        }

        string? major = null;
        string? minor = null;
        if (account.IsApplicationSubmitted)
        {
            var draftEntity = await _applications.GetDraftByAccountIdAsync(applicantAccountId, cancellationToken);
            if (draftEntity is not null)
            {
                var draft = JsonSerializer.Deserialize<ApplicantApplicationDraftDto>(draftEntity.Data, _json);
                major = draft?.Courses?.MajorSubject;
                minor = draft?.Courses?.MinorSubject;
            }
        }

        var studentNumber = BuildStudentNumber(account, opt);
        var taken = await _students.GetByStudentNumberAsync(studentNumber, cancellationToken);
        if (taken is not null)
        {
            studentNumber = $"{studentNumber[..Math.Min(20, studentNumber.Length)]}-{Guid.NewGuid().ToString("N")[..6]}".ToUpperInvariant();
        }

        try
        {
            var student = new Student(
                applicantAccountId,
                studentNumber,
                account.FullName,
                account.DateOfBirth,
                account.Gender,
                account.Email,
                account.MobileNumber,
                account.Shift,
                opt.AcademicYear.Trim(),
                opt.DefaultProgramId,
                opt.DefaultProgramCode,
                major,
                minor,
                account.UniqueId,
                account.PhotoUrl,
                createdBy: "admission-erp-sync");

            await _students.AddAsync(student, cancellationToken);
            await _students.SaveChangesAsync(cancellationToken);

            account = await _accounts.GetByIdForUpdateAsync(applicantAccountId, cancellationToken)
                      ?? throw new InvalidOperationException("Account disappeared after student insert.");
            account.RecordErpStudentLink(student.Id, DateTime.UtcNow);
            await _accounts.UpdateAsync(account, cancellationToken);

            _logger.LogInformation(
                "Admission ERP sync: created Student {StudentId} for applicant {AccountId} ({UniqueId}).",
                student.Id,
                applicantAccountId,
                account.UniqueId);

            return new AdmissionErpSyncResult(true, student.Id, "Student created in ERP.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Admission ERP sync failed for {AccountId}", applicantAccountId);
            account = await _accounts.GetByIdForUpdateAsync(applicantAccountId, cancellationToken);
            if (account is not null)
            {
                account.RecordErpSyncFailure(ex.Message);
                await _accounts.UpdateAsync(account, cancellationToken);
            }

            return new AdmissionErpSyncResult(false, null, ex.Message);
        }
    }

    private static string BuildStudentNumber(StudentApplicantAccount account, AdmissionErpSyncOptions opt)
    {
        if (opt.UseApplicationNumberAsStudentNumber && !string.IsNullOrWhiteSpace(account.UniqueId))
        {
            var u = account.UniqueId.Trim().ToUpperInvariant();
            return u.Length > 32 ? u[..32] : u;
        }

        var g = Guid.NewGuid().ToString("N");
        var s = $"S{g}";
        return s.Length > 32 ? s[..32] : s;
    }
}
