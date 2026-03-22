namespace ERP.Domain.Hostel.Entities;

public class RoomAllocation
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid RoomId { get; private set; }

    public Guid StudentId { get; private set; }

    public DateTime AllocationDate { get; private set; }

    public DateTime? VacatedDate { get; private set; }

    public AllocationStatus Status { get; private set; } = AllocationStatus.Active;

    public decimal? MonthlyRent { get; private set; }

    public string? BedNumber { get; private set; } // Optional bed number within the room

    public string? Remarks { get; private set; }

    public string? AllocatedBy { get; private set; }

    public string? VacatedBy { get; private set; }

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public DateTime? UpdatedOnUtc { get; private set; }

    private RoomAllocation() { } // For EF Core

    public RoomAllocation(
        Guid roomId,
        Guid studentId,
        DateTime allocationDate,
        decimal? monthlyRent = null,
        string? bedNumber = null,
        string? remarks = null,
        string? allocatedBy = null)
    {
        RoomId = roomId;
        StudentId = studentId;
        AllocationDate = allocationDate;
        MonthlyRent = monthlyRent;
        BedNumber = bedNumber;
        Remarks = remarks;
        Status = AllocationStatus.Active;
        AllocatedBy = allocatedBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Vacate(DateTime vacatedDate, string? vacatedBy = null, string? remarks = null)
    {
        if (Status != AllocationStatus.Active)
        {
            throw new InvalidOperationException("Only active allocations can be vacated.");
        }

        VacatedDate = vacatedDate;
        Status = AllocationStatus.Vacated;
        VacatedBy = vacatedBy;
        if (!string.IsNullOrWhiteSpace(remarks))
        {
            Remarks = remarks;
        }
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateRent(decimal monthlyRent)
    {
        MonthlyRent = monthlyRent;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum AllocationStatus
{
    Active,
    Vacated,
    Transferred
}




