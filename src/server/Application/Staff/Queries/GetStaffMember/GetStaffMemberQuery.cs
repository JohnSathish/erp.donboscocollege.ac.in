using MediatR;

namespace ERP.Application.Staff.Queries.GetStaffMember;

public sealed record GetStaffMemberQuery(Guid Id) : IRequest<StaffMemberDto?>;

public sealed record StaffMemberDto(
    Guid Id,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string MobileNumber,
    string DateOfBirth, // Changed from DateOnly to string for JSON serialization
    string Gender,
    string? Department,
    string? Designation,
    string? EmployeeType,
    string JoinDate, // Changed from DateOnly to string for JSON serialization
    string? ExitDate, // Changed from DateOnly? to string? for JSON serialization
    string Status,
    string? Address,
    string? EmergencyContactName,
    string? EmergencyContactNumber,
    string? Qualifications,
    string? Specialization,
    DateTime CreatedOnUtc,
    string? CreatedBy,
    DateTime? UpdatedOnUtc,
    string? UpdatedBy);

