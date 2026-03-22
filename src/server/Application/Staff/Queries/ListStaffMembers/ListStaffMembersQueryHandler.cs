using ERP.Application.Staff.Interfaces;
using MediatR;

namespace ERP.Application.Staff.Queries.ListStaffMembers;

public sealed class ListStaffMembersQueryHandler : IRequestHandler<ListStaffMembersQuery, ListStaffMembersResult>
{
    private readonly IStaffRepository _staffRepository;

    public ListStaffMembersQueryHandler(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    public async Task<ListStaffMembersResult> Handle(ListStaffMembersQuery request, CancellationToken cancellationToken)
    {
        var (staff, totalCount) = await _staffRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.Department,
            request.EmployeeType,
            request.Status,
            request.SearchTerm,
            cancellationToken);

        var staffDtos = staff.Select(s => new StaffMemberDto(
            s.Id,
            s.EmployeeNumber,
            s.FirstName,
            s.LastName,
            s.FullName,
            s.Email,
            s.MobileNumber,
            s.DateOfBirth,
            s.Gender,
            s.Department,
            s.Designation,
            s.EmployeeType,
            s.JoinDate,
            s.ExitDate,
            s.Status.ToString(),
            s.Address,
            s.EmergencyContactName,
            s.EmergencyContactNumber,
            s.Qualifications,
            s.Specialization,
            s.CreatedOnUtc,
            s.CreatedBy)).ToList();

        return new ListStaffMembersResult(
            staffDtos,
            totalCount,
            request.Page,
            request.PageSize);
    }
}




