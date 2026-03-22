namespace ERP.Domain.Admissions.Entities;

public class EntranceExam
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string ExamName { get; private set; }

    public string ExamCode { get; private set; }

    public string? Description { get; private set; }

    public DateTime ExamDate { get; private set; }

    public TimeOnly ExamStartTime { get; private set; }

    public TimeOnly ExamEndTime { get; private set; }

    public string Venue { get; private set; }

    public string? VenueAddress { get; private set; }

    public string? Instructions { get; private set; }

    public int MaxCapacity { get; private set; }

    public int CurrentRegistrations { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    private readonly List<ExamRegistration> _registrations = new();
    public IReadOnlyCollection<ExamRegistration> Registrations => _registrations.AsReadOnly();

    private EntranceExam()
    {
        ExamName = string.Empty;
        ExamCode = string.Empty;
        Venue = string.Empty;
    }

    public EntranceExam(
        string examName,
        string examCode,
        DateTime examDate,
        TimeOnly examStartTime,
        TimeOnly examEndTime,
        string venue,
        int maxCapacity,
        string? description = null,
        string? venueAddress = null,
        string? instructions = null,
        string? createdBy = null)
    {
        ExamName = examName.Trim();
        ExamCode = examCode.Trim().ToUpperInvariant();
        // Ensure ExamDate is UTC (PostgreSQL requirement)
        ExamDate = examDate.Kind == DateTimeKind.Unspecified 
            ? DateTime.SpecifyKind(examDate.Date, DateTimeKind.Utc)
            : examDate.ToUniversalTime().Date;
        ExamStartTime = examStartTime;
        ExamEndTime = examEndTime;
        Venue = venue.Trim();
        MaxCapacity = maxCapacity;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        VenueAddress = string.IsNullOrWhiteSpace(venueAddress) ? null : venueAddress.Trim();
        Instructions = string.IsNullOrWhiteSpace(instructions) ? null : instructions.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CurrentRegistrations = 0;
        IsActive = true;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(
        string examName,
        DateTime examDate,
        TimeOnly examStartTime,
        TimeOnly examEndTime,
        string venue,
        int maxCapacity,
        string? description = null,
        string? venueAddress = null,
        string? instructions = null,
        string? updatedBy = null)
    {
        ExamName = examName.Trim();
        // Ensure ExamDate is UTC (PostgreSQL requirement)
        ExamDate = examDate.Kind == DateTimeKind.Unspecified 
            ? DateTime.SpecifyKind(examDate.Date, DateTimeKind.Utc)
            : examDate.ToUniversalTime().Date;
        ExamStartTime = examStartTime;
        ExamEndTime = examEndTime;
        Venue = venue.Trim();
        MaxCapacity = maxCapacity;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        VenueAddress = string.IsNullOrWhiteSpace(venueAddress) ? null : venueAddress.Trim();
        Instructions = string.IsNullOrWhiteSpace(instructions) ? null : instructions.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void SetActiveStatus(bool isActive, string? updatedBy = null)
    {
        IsActive = isActive;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public bool CanRegister()
    {
        return IsActive && CurrentRegistrations < MaxCapacity && ExamDate >= DateTime.UtcNow.Date;
    }

    public void IncrementRegistrations()
    {
        if (CurrentRegistrations >= MaxCapacity)
        {
            throw new InvalidOperationException($"Exam {ExamCode} has reached maximum capacity.");
        }
        CurrentRegistrations++;
    }

    public void DecrementRegistrations()
    {
        if (CurrentRegistrations <= 0)
        {
            return;
        }
        CurrentRegistrations--;
    }

    public void AddRegistration(ExamRegistration registration)
    {
        if (!CanRegister())
        {
            throw new InvalidOperationException($"Cannot register for exam {ExamCode}. Exam is inactive, full, or has passed.");
        }
        _registrations.Add(registration);
        IncrementRegistrations();
    }
}



