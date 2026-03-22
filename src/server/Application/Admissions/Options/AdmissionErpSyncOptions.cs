namespace ERP.Application.Admissions.Options;

/// <summary>Maps approved online applications to ERP student records.</summary>
public sealed class AdmissionErpSyncOptions
{
    public const string SectionName = "AdmissionErpSync";

    /// <summary>When false, approval still succeeds but no student row is created.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Required for provisioning a <see cref="ERP.Domain.Students.Entities.Student"/>.</summary>
    public string AcademicYear { get; set; } = "2025-2026";

    public Guid? DefaultProgramId { get; set; }

    public string? DefaultProgramCode { get; set; }

    /// <summary>If true, use applicant <c>UniqueId</c> as <c>StudentNumber</c> when possible.</summary>
    public bool UseApplicationNumberAsStudentNumber { get; set; } = true;
}
