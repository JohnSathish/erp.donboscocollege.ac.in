using MediatR;

namespace ERP.Application.Staff.Commands.UpdateStaffMember;

public sealed record UpdateStaffMemberCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string MobileNumber,
    string? Department,
    string? Designation,
    string? EmployeeType,
    string? Address = null,
    string? EmergencyContactName = null,
    string? EmergencyContactNumber = null,
    string? Qualifications = null,
    string? Specialization = null,
    string? UpdatedBy = null) : IRequest<Unit>;

