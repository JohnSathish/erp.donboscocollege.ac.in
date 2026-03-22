using ERP.Application.Staff.Commands.CreateStaffMember;
using ERP.Application.Staff.Commands.UpdateStaffMember;
using ERP.Application.Staff.Queries.GetStaffMember;
using ERP.Application.Staff.Queries.ListStaffMembers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StaffController> _logger;

    public StaffController(IMediator mediator, ILogger<StaffController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ListStaffMembersResult>> ListStaffMembers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? department = null,
        [FromQuery] string? employeeType = null,
        [FromQuery] string? status = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListStaffMembersQuery(
            page,
            pageSize,
            department,
            employeeType,
            status != null && Enum.TryParse<ERP.Domain.Staff.Entities.StaffStatus>(status, true, out var parsedStatus) ? parsedStatus : null,
            searchTerm);

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ERP.Application.Staff.Queries.GetStaffMember.StaffMemberDto>> GetStaffMember(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetStaffMemberQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateStaffMember(
        [FromBody] CreateStaffMemberRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateStaffMemberCommand(
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
                request.Address,
                request.EmergencyContactName,
                request.EmergencyContactNumber,
                request.Qualifications,
                request.Specialization,
                request.CreatedBy);

            var staffId = await _mediator.Send(command, cancellationToken);
            return Ok(staffId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating staff member");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStaffMember(
        Guid id,
        [FromBody] UpdateStaffMemberRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateStaffMemberCommand(
                id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.MobileNumber,
                request.Department,
                request.Designation,
                request.EmployeeType,
                request.Address,
                request.EmergencyContactName,
                request.EmergencyContactNumber,
                request.Qualifications,
                request.Specialization,
                request.UpdatedBy);

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error updating staff member {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating staff member {Id}", id);
            return BadRequest(new { message = ex.Message });
        }
    }
}

// Request DTOs
public record CreateStaffMemberRequest(
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
    string? CreatedBy = null);

public record UpdateStaffMemberRequest(
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
    string? UpdatedBy = null);

