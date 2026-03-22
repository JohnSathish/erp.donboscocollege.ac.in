using MediatR;
using ERP.Domain.Staff.Entities;

namespace ERP.Application.Staff.Queries.ListStaffMembers;

public sealed record ListStaffMembersQuery(
    int Page = 1,
    int PageSize = 50,
    string? Department = null,
    string? EmployeeType = null,
    StaffStatus? Status = null,
    string? SearchTerm = null) : IRequest<ListStaffMembersResult>;

public sealed record ListStaffMembersResult(
    IReadOnlyCollection<StaffMemberDto> Staff,
    int TotalCount,
    int Page,
    int PageSize);

public sealed record StaffMemberDto(
    Guid Id,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string MobileNumber,
    DateOnly DateOfBirth,
    string Gender,
    string? Department,
    string? Designation,
    string? EmployeeType,
    DateOnly JoinDate,
    DateOnly? ExitDate,
    string Status,
    string? Address,
    string? EmergencyContactName,
    string? EmergencyContactNumber,
    string? Qualifications,
    string? Specialization,
    DateTime CreatedOnUtc,
    string? CreatedBy);




