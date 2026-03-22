namespace ERP.Domain.Admissions.Entities;

public class ExamRegistration
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid ExamId { get; private set; }

    public EntranceExam Exam { get; private set; } = null!;

    public Guid ApplicantAccountId { get; private set; }

    public StudentApplicantAccount ApplicantAccount { get; private set; } = null!;

    public string? HallTicketNumber { get; private set; }

    public bool IsPresent { get; private set; } = false;

    public decimal? Score { get; private set; }

    public DateTime RegisteredOnUtc { get; private set; } = DateTime.UtcNow;

    public string? RegisteredBy { get; private set; }

    public DateTime? AttendanceMarkedOnUtc { get; private set; }

    public string? AttendanceMarkedBy { get; private set; }

    public DateTime? ScoreEnteredOnUtc { get; private set; }

    public string? ScoreEnteredBy { get; private set; }

    private ExamRegistration()
    {
    }

    public ExamRegistration(
        Guid examId,
        Guid applicantAccountId,
        string? registeredBy = null)
    {
        ExamId = examId;
        ApplicantAccountId = applicantAccountId;
        RegisteredBy = string.IsNullOrWhiteSpace(registeredBy) ? null : registeredBy.Trim();
        RegisteredOnUtc = DateTime.UtcNow;
    }

    public void GenerateHallTicketNumber(string examCode, string applicantUniqueId)
    {
        // Format: HT-{ExamCode}-{ApplicantUniqueId}-{Last4DigitsOfId}
        var idSuffix = Id.ToString().Substring(Id.ToString().Length - 4);
        HallTicketNumber = $"HT-{examCode}-{applicantUniqueId}-{idSuffix}";
    }

    public void MarkAttendance(bool isPresent, string? markedBy = null)
    {
        IsPresent = isPresent;
        AttendanceMarkedBy = string.IsNullOrWhiteSpace(markedBy) ? null : markedBy.Trim();
        AttendanceMarkedOnUtc = DateTime.UtcNow;
    }

    public void EnterScore(decimal score, string? enteredBy = null)
    {
        if (score < 0)
        {
            throw new ArgumentException("Score cannot be negative.", nameof(score));
        }
        Score = score;
        ScoreEnteredBy = string.IsNullOrWhiteSpace(enteredBy) ? null : enteredBy.Trim();
        ScoreEnteredOnUtc = DateTime.UtcNow;
    }
}

