using MediatR;

namespace ERP.Application.Staff.Commands.CreateStaffMember;

public sealed record CreateStaffMemberCommand(
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Email,
    string MobileNumber,
    DateOnly DateOfBirth,
    string Gender,
    string? Department,
    string? Designation,
    string? EmployeeType,
    DateOnly JoinDate,
    string? Address = null,
    string? EmergencyContactName = null,
    string? EmergencyContactNumber = null,
    string? Qualifications = null,
    string? Specialization = null,
    string? CreatedBy = null) : IRequest<Guid>;




