namespace ERP.Domain.Fees.Entities;

public class Scholarship
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public string ScholarshipName { get; private set; } = string.Empty;
    public ScholarshipType Type { get; private set; }
    public decimal? Percentage { get; private set; } // Percentage discount (e.g., 50%)
    public decimal? FixedAmount { get; private set; } // Fixed amount discount
    public string AcademicYear { get; private set; } = string.Empty;
    public DateTime EffectiveFrom { get; private set; }
    public DateTime? EffectiveTo { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? Description { get; private set; }
    public string? SponsorName { get; private set; }
    public string? ApprovalReference { get; private set; }
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private Scholarship() { }

    public Scholarship(
        Guid studentId,
        string scholarshipName,
        ScholarshipType type,
        string academicYear,
        DateTime effectiveFrom,
        decimal? percentage = null,
        decimal? fixedAmount = null,
        DateTime? effectiveTo = null,
        string? description = null,
        string? sponsorName = null,
        string? approvalReference = null,
        string? remarks = null,
        string? createdBy = null)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (string.IsNullOrWhiteSpace(scholarshipName))
            throw new ArgumentException("Scholarship name is required.", nameof(scholarshipName));
        if (string.IsNullOrWhiteSpace(academicYear))
            throw new ArgumentException("Academic year is required.", nameof(academicYear));
        if (type == ScholarshipType.Percentage && !percentage.HasValue)
            throw new ArgumentException("Percentage is required for percentage-based scholarships.", nameof(percentage));
        if (type == ScholarshipType.FixedAmount && !fixedAmount.HasValue)
            throw new ArgumentException("Fixed amount is required for fixed-amount scholarships.", nameof(fixedAmount));
        if (percentage.HasValue && (percentage < 0 || percentage > 100))
            throw new ArgumentException("Percentage must be between 0 and 100.", nameof(percentage));
        if (fixedAmount.HasValue && fixedAmount < 0)
            throw new ArgumentException("Fixed amount cannot be negative.", nameof(fixedAmount));

        StudentId = studentId;
        ScholarshipName = scholarshipName.Trim();
        Type = type;
        Percentage = percentage;
        FixedAmount = fixedAmount;
        AcademicYear = academicYear.Trim();
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        SponsorName = string.IsNullOrWhiteSpace(sponsorName) ? null : sponsorName.Trim();
        ApprovalReference = string.IsNullOrWhiteSpace(approvalReference) ? null : approvalReference.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public decimal CalculateDiscountAmount(decimal invoiceAmount)
    {
        if (!IsActive)
            return 0;

        var now = DateTime.UtcNow;
        if (now < EffectiveFrom || (EffectiveTo.HasValue && now > EffectiveTo.Value))
            return 0;

        return Type switch
        {
            ScholarshipType.Percentage => Percentage.HasValue ? invoiceAmount * (Percentage.Value / 100) : 0,
            ScholarshipType.FixedAmount => FixedAmount ?? 0,
            _ => 0
        };
    }

    public void Deactivate(string? updatedBy = null)
    {
        IsActive = false;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Activate(string? updatedBy = null)
    {
        IsActive = true;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Update(
        string scholarshipName,
        DateTime effectiveFrom,
        DateTime? effectiveTo = null,
        decimal? percentage = null,
        decimal? fixedAmount = null,
        string? description = null,
        string? sponsorName = null,
        string? approvalReference = null,
        string? remarks = null,
        string? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(scholarshipName))
            throw new ArgumentException("Scholarship name is required.", nameof(scholarshipName));
        if (Type == ScholarshipType.Percentage && !percentage.HasValue)
            throw new ArgumentException("Percentage is required for percentage-based scholarships.", nameof(percentage));
        if (Type == ScholarshipType.FixedAmount && !fixedAmount.HasValue)
            throw new ArgumentException("Fixed amount is required for fixed-amount scholarships.", nameof(fixedAmount));
        if (percentage.HasValue && (percentage < 0 || percentage > 100))
            throw new ArgumentException("Percentage must be between 0 and 100.", nameof(percentage));
        if (fixedAmount.HasValue && fixedAmount < 0)
            throw new ArgumentException("Fixed amount cannot be negative.", nameof(fixedAmount));

        ScholarshipName = scholarshipName.Trim();
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        Percentage = percentage;
        FixedAmount = fixedAmount;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        SponsorName = string.IsNullOrWhiteSpace(sponsorName) ? null : sponsorName.Trim();
        ApprovalReference = string.IsNullOrWhiteSpace(approvalReference) ? null : approvalReference.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum ScholarshipType
{
    Percentage = 0,
    FixedAmount = 1
}

