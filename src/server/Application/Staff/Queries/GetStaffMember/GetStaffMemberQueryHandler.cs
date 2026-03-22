using ERP.Application.Staff.Interfaces;
using MediatR;

namespace ERP.Application.Staff.Queries.GetStaffMember;

public sealed class GetStaffMemberQueryHandler : IRequestHandler<GetStaffMemberQuery, StaffMemberDto?>
{
    private readonly IStaffRepository _staffRepository;

    public GetStaffMemberQueryHandler(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    public async Task<StaffMemberDto?> Handle(GetStaffMemberQuery request, CancellationToken cancellationToken)
    {
        var staff = await _staffRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (staff == null)
        {
            return null;
        }

        return new StaffMemberDto(
            staff.Id,
            staff.EmployeeNumber,
            staff.FirstName,
            staff.LastName,
            staff.FullName,
            staff.Email,
            staff.MobileNumber,
            staff.DateOfBirth.ToString("yyyy-MM-dd"), // Convert DateOnly to string
            staff.Gender,
            staff.Department,
            staff.Designation,
            staff.EmployeeType,
            staff.JoinDate.ToString("yyyy-MM-dd"), // Convert DateOnly to string
            staff.ExitDate?.ToString("yyyy-MM-dd"), // Convert DateOnly? to string?
            staff.Status.ToString(),
            staff.Address,
            staff.EmergencyContactName,
            staff.EmergencyContactNumber,
            staff.Qualifications,
            staff.Specialization,
            staff.CreatedOnUtc,
            staff.CreatedBy,
            staff.UpdatedOnUtc,
            staff.UpdatedBy);
    }
}

