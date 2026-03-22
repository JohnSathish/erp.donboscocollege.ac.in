namespace ERP.Domain.Admissions.Entities;

public class FeeComponent
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid FeeStructureId { get; private set; }

    public FeeStructure FeeStructure { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public decimal Amount { get; private set; }

    public bool IsOptional { get; private set; } = false;

    public int? InstallmentNumber { get; private set; } // For installment-based fees

    public DateTime? DueDateUtc { get; private set; }

    public int DisplayOrder { get; private set; }

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    private FeeComponent()
    {
    }

    public FeeComponent(
        Guid feeStructureId,
        string name,
        decimal amount,
        bool isOptional = false,
        int? installmentNumber = null,
        DateTime? dueDateUtc = null,
        string? description = null,
        int displayOrder = 0,
        string? createdBy = null)
    {
        if (feeStructureId == Guid.Empty)
            throw new ArgumentException("Fee structure ID is required.", nameof(feeStructureId));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Component name is required.", nameof(name));
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        FeeStructureId = feeStructureId;
        Name = name.Trim();
        Amount = amount;
        IsOptional = isOptional;
        InstallmentNumber = installmentNumber;
        DueDateUtc = dueDateUtc;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        DisplayOrder = displayOrder;
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(
        string name,
        decimal amount,
        bool isOptional = false,
        int? installmentNumber = null,
        DateTime? dueDateUtc = null,
        string? description = null,
        int displayOrder = 0,
        string? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Component name is required.", nameof(name));
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        Name = name.Trim();
        Amount = amount;
        IsOptional = isOptional;
        InstallmentNumber = installmentNumber;
        DueDateUtc = dueDateUtc;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        DisplayOrder = displayOrder;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}









