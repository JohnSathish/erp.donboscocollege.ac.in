namespace ERP.Domain.Hostel.Entities;

public class HostelRoom
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string RoomNumber { get; private set; } = string.Empty;

    public string BlockName { get; private set; } = string.Empty; // e.g., "Block A", "Block B"

    public string FloorNumber { get; private set; } = string.Empty;

    public int Capacity { get; private set; } // Number of beds

    public int OccupiedBeds { get; private set; }

    public int AvailableBeds => Capacity - OccupiedBeds;

    public string RoomType { get; private set; } = string.Empty; // Single, Double, Triple, Dormitory

    public decimal? MonthlyRent { get; private set; }

    public string? Facilities { get; private set; } // Comma-separated list of facilities

    public RoomStatus Status { get; private set; } = RoomStatus.Available;

    public string? Notes { get; private set; }

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    private HostelRoom() { } // For EF Core

    public HostelRoom(
        string roomNumber,
        string blockName,
        string floorNumber,
        int capacity,
        string roomType,
        decimal? monthlyRent = null,
        string? facilities = null,
        string? notes = null,
        string? createdBy = null)
    {
        RoomNumber = roomNumber;
        BlockName = blockName;
        FloorNumber = floorNumber;
        Capacity = capacity;
        RoomType = roomType;
        MonthlyRent = monthlyRent;
        Facilities = facilities;
        Notes = notes;
        Status = RoomStatus.Available;
        OccupiedBeds = 0;
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string blockName,
        string floorNumber,
        int capacity,
        string roomType,
        decimal? monthlyRent,
        string? facilities,
        string? notes,
        string? updatedBy = null)
    {
        if (capacity < OccupiedBeds)
        {
            throw new InvalidOperationException(
                $"Cannot reduce capacity below current occupied beds ({OccupiedBeds}).");
        }

        BlockName = blockName;
        FloorNumber = floorNumber;
        Capacity = capacity;
        RoomType = roomType;
        MonthlyRent = monthlyRent;
        Facilities = facilities;
        Notes = notes;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;

        // Update status based on availability
        if (AvailableBeds == 0)
        {
            Status = RoomStatus.Full;
        }
        else if (AvailableBeds < Capacity)
        {
            Status = RoomStatus.PartiallyOccupied;
        }
        else
        {
            Status = RoomStatus.Available;
        }
    }

    public void AllocateBed()
    {
        if (AvailableBeds <= 0)
        {
            throw new InvalidOperationException("No available beds in this room.");
        }

        OccupiedBeds++;
        UpdatedOnUtc = DateTime.UtcNow;

        if (AvailableBeds == 0)
        {
            Status = RoomStatus.Full;
        }
        else
        {
            Status = RoomStatus.PartiallyOccupied;
        }
    }

    public void VacateBed()
    {
        if (OccupiedBeds <= 0)
        {
            throw new InvalidOperationException("No beds are occupied in this room.");
        }

        OccupiedBeds--;
        UpdatedOnUtc = DateTime.UtcNow;

        if (AvailableBeds == Capacity)
        {
            Status = RoomStatus.Available;
        }
        else
        {
            Status = RoomStatus.PartiallyOccupied;
        }
    }

    public void UpdateStatus(RoomStatus status, string? updatedBy = null)
    {
        Status = status;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum RoomStatus
{
    Available,
    PartiallyOccupied,
    Full,
    UnderMaintenance,
    Reserved
}




