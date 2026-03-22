namespace ERP.Domain.Admissions.Entities;

/// <summary>Reference rows for Class XII subject dropdowns (MBOSE/CBSE/ISC × Arts/Science/Commerce). Not used for "Other" board.</summary>
public sealed class ClassXiiSubjectMaster
{
    public Guid Id { get; set; }

    /// <summary>Uppercase code: MBOSE, CBSE, ISC.</summary>
    public string BoardCode { get; set; } = string.Empty;

    /// <summary>Uppercase code: ARTS, SCIENCE, COMMERCE.</summary>
    public string StreamCode { get; set; } = string.Empty;

    public string SubjectName { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;
}
