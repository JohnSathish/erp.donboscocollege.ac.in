namespace ERP.Domain.Staff.Entities;

public class StaffMember
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string EmployeeNumber { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}".Trim();

    public string Email { get; private set; } = string.Empty;

    public string MobileNumber { get; private set; } = string.Empty;

    public DateOnly DateOfBirth { get; private set; }

    public string Gender { get; private set; } = string.Empty;

    public string? Department { get; private set; }

    public string? Designation { get; private set; }

    public string? EmployeeType { get; private set; } // Teaching, Non-Teaching, Administrative

    public DateOnly JoinDate { get; private set; }

    public DateOnly? ExitDate { get; private set; }

    public StaffStatus Status { get; private set; } = StaffStatus.Active;

    public string? Address { get; private set; }

    public string? EmergencyContactName { get; private set; }

    public string? EmergencyContactNumber { get; private set; }

    public string? Qualifications { get; private set; }

    public string? Specialization { get; private set; }

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    private StaffMember() { } // For EF Core

    public StaffMember(
        string employeeNumber,
        string firstName,
        string lastName,
        string email,
        string mobileNumber,
        DateOnly dateOfBirth,
        string gender,
        string? department,
        string? designation,
        string? employeeType,
        DateOnly joinDate,
        string? createdBy = null)
    {
        EmployeeNumber = employeeNumber;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        MobileNumber = mobileNumber;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        Department = department;
        Designation = designation;
        EmployeeType = employeeType;
        JoinDate = joinDate;
        Status = StaffStatus.Active;
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdatePersonalInfo(
        string firstName,
        string lastName,
        string email,
        string mobileNumber,
        string? address,
        string? emergencyContactName,
        string? emergencyContactNumber,
        string? updatedBy = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        MobileNumber = mobileNumber;
        Address = address;
        EmergencyContactName = emergencyContactName;
        EmergencyContactNumber = emergencyContactNumber;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateEmploymentInfo(
        string? department,
        string? designation,
        string? employeeType,
        string? updatedBy = null)
    {
        Department = department;
        Designation = designation;
        EmployeeType = employeeType;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateQualifications(
        string? qualifications,
        string? specialization,
        string? updatedBy = null)
    {
        Qualifications = qualifications;
        Specialization = specialization;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateStatus(StaffStatus status, string? updatedBy = null)
    {
        Status = status;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;

        if (status == StaffStatus.Resigned || status == StaffStatus.Terminated)
        {
            ExitDate = DateOnly.FromDateTime(DateTime.UtcNow);
        }
    }

    public void Resign(DateOnly exitDate, string? updatedBy = null)
    {
        Status = StaffStatus.Resigned;
        ExitDate = exitDate;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Terminate(DateOnly exitDate, string? updatedBy = null)
    {
        Status = StaffStatus.Terminated;
        ExitDate = exitDate;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum StaffStatus
{
    Active,
    OnLeave,
    Suspended,
    Resigned,
    Terminated
}




