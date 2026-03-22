namespace ERP.Domain.Students.Entities;

public class DisciplineRecord
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public string IncidentType { get; private set; } = string.Empty; // e.g., "Academic Misconduct", "Behavioral Issue", "Attendance Violation"
    public string Description { get; private set; } = string.Empty;
    public DateTime IncidentDate { get; private set; }
    public string? Location { get; private set; }
    public string? ReportedBy { get; private set; }
    public string? Witnesses { get; private set; }
    public DisciplineSeverity Severity { get; private set; } = DisciplineSeverity.Low;
    public DisciplineAction ActionTaken { get; private set; } = DisciplineAction.Warning;
    public string? ActionDetails { get; private set; }
    public DateTime? ActionDate { get; private set; }
    public string? ActionTakenBy { get; private set; }
    public bool IsResolved { get; private set; } = false;
    public DateTime? ResolvedOnUtc { get; private set; }
    public string? ResolutionNotes { get; private set; }
    public string? Remarks { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

    private DisciplineRecord() { }

    public DisciplineRecord(
        Guid studentId,
        string incidentType,
        string description,
        DateTime incidentDate,
        DisciplineSeverity severity,
        string? location = null,
        string? reportedBy = null,
        string? witnesses = null,
        string? createdBy = null)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (string.IsNullOrWhiteSpace(incidentType))
            throw new ArgumentException("Incident type is required.", nameof(incidentType));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        StudentId = studentId;
        IncidentType = incidentType.Trim();
        Description = description.Trim();
        IncidentDate = incidentDate;
        Severity = severity;
        Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
        ReportedBy = string.IsNullOrWhiteSpace(reportedBy) ? null : reportedBy.Trim();
        Witnesses = string.IsNullOrWhiteSpace(witnesses) ? null : witnesses.Trim();
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void RecordAction(
        DisciplineAction action,
        string actionDetails,
        string actionTakenBy,
        DateTime? actionDate = null)
    {
        if (string.IsNullOrWhiteSpace(actionDetails))
            throw new ArgumentException("Action details are required.", nameof(actionDetails));
        if (string.IsNullOrWhiteSpace(actionTakenBy))
            throw new ArgumentException("Action taken by is required.", nameof(actionTakenBy));

        ActionTaken = action;
        ActionDetails = actionDetails.Trim();
        ActionTakenBy = actionTakenBy.Trim();
        ActionDate = actionDate ?? DateTime.UtcNow;
        UpdatedBy = actionTakenBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Resolve(string resolvedBy, string resolutionNotes)
    {
        if (string.IsNullOrWhiteSpace(resolutionNotes))
            throw new ArgumentException("Resolution notes are required.", nameof(resolutionNotes));

        IsResolved = true;
        ResolvedOnUtc = DateTime.UtcNow;
        ResolutionNotes = resolutionNotes.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(resolvedBy) ? null : resolvedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateRemarks(string remarks, string? updatedBy = null)
    {
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum DisciplineSeverity
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

public enum DisciplineAction
{
    Warning = 0,
    VerbalWarning = 1,
    WrittenWarning = 2,
    Suspension = 3,
    Probation = 4,
    Dismissal = 5,
    Other = 6
}

