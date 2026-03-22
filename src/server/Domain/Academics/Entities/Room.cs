namespace ERP.Domain.Academics.Entities;

public class Room
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string RoomNumber { get; private set; } = string.Empty;
    public string? Building { get; private set; }
    public string? Floor { get; private set; }
    public RoomType Type { get; private set; } = RoomType.Classroom;
    public int Capacity { get; private set; }
    public bool HasProjector { get; private set; } = false;
    public bool HasComputerLab { get; private set; } = false;
    public bool HasWhiteboard { get; private set; } = true;
    public string? Equipment { get; private set; } // JSON or comma-separated list
    public bool IsActive { get; private set; } = true;
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private Room() { }

    public Room(
        string roomNumber,
        RoomType type,
        int capacity,
        string? building = null,
        string? floor = null,
        bool hasProjector = false,
        bool hasComputerLab = false,
        bool hasWhiteboard = true,
        string? equipment = null,
        string? remarks = null,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(roomNumber))
            throw new ArgumentException("Room number is required.", nameof(roomNumber));
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));

        RoomNumber = roomNumber.Trim();
        Type = type;
        Capacity = capacity;
        Building = string.IsNullOrWhiteSpace(building) ? null : building.Trim();
        Floor = string.IsNullOrWhiteSpace(floor) ? null : floor.Trim();
        HasProjector = hasProjector;
        HasComputerLab = hasComputerLab;
        HasWhiteboard = hasWhiteboard;
        Equipment = string.IsNullOrWhiteSpace(equipment) ? null : equipment.Trim();
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateCapacity(int capacity, string? updatedBy = null)
    {
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));

        Capacity = capacity;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateEquipment(string? equipment, string? updatedBy = null)
    {
        Equipment = string.IsNullOrWhiteSpace(equipment) ? null : equipment.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Deactivate(string? updatedBy = null)
    {
        IsActive = false;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum RoomType
{
    Classroom = 0,
    Laboratory = 1,
    ComputerLab = 2,
    Library = 3,
    Auditorium = 4,
    ConferenceRoom = 5,
    SportsHall = 6,
    Other = 7
}

