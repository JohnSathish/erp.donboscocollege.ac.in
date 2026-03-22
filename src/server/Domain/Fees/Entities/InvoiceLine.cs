namespace ERP.Domain.Fees.Entities;

public class InvoiceLine
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid InvoiceId { get; private set; }
    public Invoice Invoice { get; private set; } = null!;
    public Guid? FeeComponentId { get; private set; } // Reference to FeeComponent if applicable
    public string Description { get; private set; } = string.Empty;
    public decimal Quantity { get; private set; } = 1;
    public decimal UnitPrice { get; private set; }
    public decimal Amount { get; private set; }
    public int DisplayOrder { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }

    private InvoiceLine() { }

    public InvoiceLine(
        Guid invoiceId,
        string description,
        decimal unitPrice,
        decimal quantity = 1,
        Guid? feeComponentId = null,
        int displayOrder = 0,
        string? createdBy = null)
    {
        if (invoiceId == Guid.Empty)
            throw new ArgumentException("Invoice ID is required.", nameof(invoiceId));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        InvoiceId = invoiceId;
        Description = description.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
        Amount = unitPrice * quantity;
        FeeComponentId = feeComponentId;
        DisplayOrder = displayOrder;
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(
        string description,
        decimal unitPrice,
        decimal quantity = 1,
        string? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        Description = description.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
        Amount = unitPrice * quantity;
    }
}

