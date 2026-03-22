namespace ERP.Domain.Students.Entities;

public class Student
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid ApplicantAccountId { get; private set; }

    public string StudentNumber { get; private set; } = string.Empty;

    public string FullName { get; private set; } = string.Empty;

    public DateOnly DateOfBirth { get; private set; }

    public string Gender { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string MobileNumber { get; private set; } = string.Empty;

    public string? PhotoUrl { get; private set; }

    public Guid? ProgramId { get; private set; }

    public string? ProgramCode { get; private set; }

    public string? MajorSubject { get; private set; }

    public string? MinorSubject { get; private set; }

    public string Shift { get; private set; } = string.Empty;

    public string AcademicYear { get; private set; } = string.Empty;

    public string? AdmissionNumber { get; private set; }

    public DateTime EnrollmentDate { get; private set; }

    public StudentStatus Status { get; private set; } = StudentStatus.Active;

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    public Guid? UserAccountId { get; private set; }

    public string? UserAccountUsername { get; private set; }

    private Student()
    {
    }

    public Student(
        Guid applicantAccountId,
        string studentNumber,
        string fullName,
        DateOnly dateOfBirth,
        string gender,
        string email,
        string mobileNumber,
        string shift,
        string academicYear,
        Guid? programId = null,
        string? programCode = null,
        string? majorSubject = null,
        string? minorSubject = null,
        string? admissionNumber = null,
        string? photoUrl = null,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(studentNumber))
            throw new ArgumentException("Student number is required.", nameof(studentNumber));
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));
        if (string.IsNullOrWhiteSpace(mobileNumber))
            throw new ArgumentException("Mobile number is required.", nameof(mobileNumber));
        if (string.IsNullOrWhiteSpace(shift))
            throw new ArgumentException("Shift is required.", nameof(shift));
        if (string.IsNullOrWhiteSpace(academicYear))
            throw new ArgumentException("Academic year is required.", nameof(academicYear));

        ApplicantAccountId = applicantAccountId;
        StudentNumber = studentNumber.Trim().ToUpperInvariant();
        FullName = fullName.Trim();
        DateOfBirth = dateOfBirth;
        Gender = gender.Trim();
        Email = email.Trim().ToLowerInvariant();
        MobileNumber = mobileNumber.Trim();
        Shift = shift.Trim();
        AcademicYear = academicYear.Trim();
        ProgramId = programId;
        ProgramCode = string.IsNullOrWhiteSpace(programCode) ? null : programCode.Trim().ToUpperInvariant();
        MajorSubject = string.IsNullOrWhiteSpace(majorSubject) ? null : majorSubject.Trim();
        MinorSubject = string.IsNullOrWhiteSpace(minorSubject) ? null : minorSubject.Trim();
        AdmissionNumber = string.IsNullOrWhiteSpace(admissionNumber) ? null : admissionNumber.Trim();
        PhotoUrl = string.IsNullOrWhiteSpace(photoUrl) ? null : photoUrl.Trim();
        EnrollmentDate = DateTime.UtcNow.Date;
        CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? null : createdBy.Trim();
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(
        string fullName,
        string email,
        string mobileNumber,
        string shift,
        Guid? programId = null,
        string? programCode = null,
        string? majorSubject = null,
        string? minorSubject = null,
        string? photoUrl = null,
        string? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));
        if (string.IsNullOrWhiteSpace(mobileNumber))
            throw new ArgumentException("Mobile number is required.", nameof(mobileNumber));
        if (string.IsNullOrWhiteSpace(shift))
            throw new ArgumentException("Shift is required.", nameof(shift));

        FullName = fullName.Trim();
        Email = email.Trim().ToLowerInvariant();
        MobileNumber = mobileNumber.Trim();
        Shift = shift.Trim();
        ProgramId = programId;
        ProgramCode = string.IsNullOrWhiteSpace(programCode) ? null : programCode.Trim().ToUpperInvariant();
        MajorSubject = string.IsNullOrWhiteSpace(majorSubject) ? null : majorSubject.Trim();
        MinorSubject = string.IsNullOrWhiteSpace(minorSubject) ? null : minorSubject.Trim();
        PhotoUrl = string.IsNullOrWhiteSpace(photoUrl) ? null : photoUrl.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateStatus(StudentStatus status, string? updatedBy = null)
    {
        Status = status;
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateAcademicYear(string academicYear, string? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(academicYear))
            throw new ArgumentException("Academic year is required.", nameof(academicYear));

        AcademicYear = academicYear.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void ChangeShift(string newShift, string? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(newShift))
            throw new ArgumentException("Shift is required.", nameof(newShift));

        Shift = newShift.Trim();
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Transfer(
        Guid? toProgramId,
        string? toProgramCode,
        string? toShift,
        string? toSection,
        string? updatedBy = null)
    {
        ProgramId = toProgramId;
        ProgramCode = string.IsNullOrWhiteSpace(toProgramCode) ? null : toProgramCode.Trim().ToUpperInvariant();
        if (!string.IsNullOrWhiteSpace(toShift))
        {
            Shift = toShift.Trim();
        }
        UpdatedBy = string.IsNullOrWhiteSpace(updatedBy) ? null : updatedBy.Trim();
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void LinkUserAccount(Guid userAccountId, string username)
    {
        if (userAccountId == Guid.Empty)
            throw new ArgumentException("User account ID cannot be empty.", nameof(userAccountId));
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.", nameof(username));

        UserAccountId = userAccountId;
        UserAccountUsername = username.Trim();
    }
}

public enum StudentStatus
{
    Active = 0,
    Inactive = 1,
    Transferred = 2,
    Graduated = 3,
    Withdrawn = 4,
    Suspended = 5
}


