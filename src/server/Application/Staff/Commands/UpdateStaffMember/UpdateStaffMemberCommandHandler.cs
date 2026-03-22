using ERP.Application.Staff.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Staff.Commands.UpdateStaffMember;

public sealed class UpdateStaffMemberCommandHandler : IRequestHandler<UpdateStaffMemberCommand, Unit>
{
    private readonly IStaffRepository _staffRepository;
    private readonly ILogger<UpdateStaffMemberCommandHandler> _logger;

    public UpdateStaffMemberCommandHandler(
        IStaffRepository staffRepository,
        ILogger<UpdateStaffMemberCommandHandler> logger)
    {
        _staffRepository = staffRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateStaffMemberCommand request, CancellationToken cancellationToken)
    {
        var staff = await _staffRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (staff == null)
        {
            throw new InvalidOperationException($"Staff member with ID '{request.Id}' not found.");
        }

        // Update personal information
        staff.UpdatePersonalInfo(
            request.FirstName,
            request.LastName,
            request.Email,
            request.MobileNumber,
            request.Address,
            request.EmergencyContactName,
            request.EmergencyContactNumber,
            request.UpdatedBy);

        // Update employment information
        staff.UpdateEmploymentInfo(
            request.Department,
            request.Designation,
            request.EmployeeType,
            request.UpdatedBy);

        // Update qualifications
        staff.UpdateQualifications(
            request.Qualifications,
            request.Specialization,
            request.UpdatedBy);

        await _staffRepository.UpdateAsync(staff, cancellationToken);

        _logger.LogInformation(
            "Updated staff member {EmployeeNumber} ({FullName})",
            staff.EmployeeNumber,
            staff.FullName);

        return Unit.Value;
    }
}

