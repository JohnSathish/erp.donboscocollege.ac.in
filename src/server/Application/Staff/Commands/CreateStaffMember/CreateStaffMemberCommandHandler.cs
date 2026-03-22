using ERP.Application.Staff.Interfaces;
using ERP.Domain.Staff.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Staff.Commands.CreateStaffMember;

public sealed class CreateStaffMemberCommandHandler : IRequestHandler<CreateStaffMemberCommand, Guid>
{
    private readonly IStaffRepository _staffRepository;
    private readonly ILogger<CreateStaffMemberCommandHandler> _logger;

    public CreateStaffMemberCommandHandler(
        IStaffRepository staffRepository,
        ILogger<CreateStaffMemberCommandHandler> logger)
    {
        _staffRepository = staffRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateStaffMemberCommand request, CancellationToken cancellationToken)
    {
        // Check if employee number already exists
        var exists = await _staffRepository.EmployeeNumberExistsAsync(request.EmployeeNumber, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException(
                $"Staff member with employee number '{request.EmployeeNumber}' already exists.");
        }

        var staffMember = new StaffMember(
            request.EmployeeNumber,
            request.FirstName,
            request.LastName,
            request.Email,
            request.MobileNumber,
            request.DateOfBirth,
            request.Gender,
            request.Department,
            request.Designation,
            request.EmployeeType,
            request.JoinDate,
            request.CreatedBy);

        if (!string.IsNullOrWhiteSpace(request.Address))
        {
            staffMember.UpdatePersonalInfo(
                request.FirstName,
                request.LastName,
                request.Email,
                request.MobileNumber,
                request.Address,
                request.EmergencyContactName,
                request.EmergencyContactNumber,
                request.CreatedBy);
        }

        if (!string.IsNullOrWhiteSpace(request.Qualifications) || !string.IsNullOrWhiteSpace(request.Specialization))
        {
            staffMember.UpdateQualifications(
                request.Qualifications,
                request.Specialization,
                request.CreatedBy);
        }

        await _staffRepository.AddAsync(staffMember, cancellationToken);

        _logger.LogInformation(
            "Created staff member {EmployeeNumber} ({FullName})",
            staffMember.EmployeeNumber,
            staffMember.FullName);

        return staffMember.Id;
    }
}




