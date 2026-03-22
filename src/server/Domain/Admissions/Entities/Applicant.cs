namespace ERP.Domain.Admissions.Entities;

public class Applicant
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string ApplicationNumber { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    public string ProgramCode { get; private set; }

    public string MobileNumber { get; private set; }

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Submitted;

    public DateTime StatusUpdatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? StatusUpdatedBy { get; private set; }

    public string? StatusRemarks { get; private set; }

    public DateTime? EntranceExamScheduledOnUtc { get; private set; }

    public string? EntranceExamVenue { get; private set; }

    public string? EntranceExamInstructions { get; private set; }

    private Applicant()
    {
        ApplicationNumber = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        ProgramCode = string.Empty;
        MobileNumber = string.Empty;
    }

    public Applicant(
        string applicationNumber,
        string firstName,
        string lastName,
        string email,
        DateOnly dateOfBirth,
        string programCode,
        string mobileNumber)
    {
        ApplicationNumber = applicationNumber;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DateOfBirth = dateOfBirth;
        ProgramCode = programCode;
        MobileNumber = mobileNumber;
    }

    public void UpdateStatus(
        ApplicationStatus status,
        string? updatedBy,
        DateTime utcNow,
        string? remarks = null,
        DateTime? entranceExamScheduledOnUtc = null,
        string? entranceExamVenue = null,
        string? entranceExamInstructions = null)
    {
        if (!IsStatusTransitionAllowed(Status, status))
        {
            throw new InvalidOperationException(
                $"Cannot transition application {ApplicationNumber} from {Status} to {status}.");
        }

        Status = status;
        StatusUpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        StatusUpdatedOnUtc = utcNow;
        StatusRemarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim();

        if (status == ApplicationStatus.EntranceExam)
        {
            EntranceExamScheduledOnUtc = entranceExamScheduledOnUtc;
            EntranceExamVenue = string.IsNullOrWhiteSpace(entranceExamVenue) ? null : entranceExamVenue.Trim();
            EntranceExamInstructions = string.IsNullOrWhiteSpace(entranceExamInstructions)
                ? null
                : entranceExamInstructions.Trim();
        }
        else
        {
            EntranceExamScheduledOnUtc = null;
            EntranceExamVenue = null;
            EntranceExamInstructions = null;
        }
    }

    private static bool IsStatusTransitionAllowed(ApplicationStatus current, ApplicationStatus next) =>
        current switch
        {
            ApplicationStatus.Submitted => next is ApplicationStatus.Approved
                or ApplicationStatus.Rejected
                or ApplicationStatus.WaitingList
                or ApplicationStatus.EntranceExam,
            ApplicationStatus.WaitingList => next is ApplicationStatus.Approved
                or ApplicationStatus.Rejected
                or ApplicationStatus.EntranceExam,
            ApplicationStatus.EntranceExam => next is ApplicationStatus.Approved
                or ApplicationStatus.Rejected
                or ApplicationStatus.WaitingList,
            _ => false
        };
}


