using ERP.Application.Attendance.Commands.CreateAttendanceSession;
using ERP.Application.Attendance.Commands.MarkAttendance;
using ERP.Application.Attendance.Commands.MarkBulkAttendance;
using ERP.Application.Attendance.Queries.GetAttendanceSummary;
using ERP.Application.Attendance.Queries.GetAbsentees;
using ERP.Application.Attendance.Interfaces;
using ERP.Domain.Attendance.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController(IMediator mediator) : ControllerBase
{
    [HttpPost("sessions")]
    [Authorize(Roles = "Admin,AcademicAdmin,Teacher")]
    public async Task<ActionResult<Guid>> CreateAttendanceSession(
        [FromBody] CreateAttendanceSessionRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateAttendanceSessionCommand(
            request.SessionName,
            request.Type,
            request.SessionDate,
            request.AcademicYear,
            request.ClassSectionId,
            request.CourseId,
            request.StaffShiftId,
            request.StartTime,
            request.EndTime,
            request.Term,
            request.Remarks,
            User?.Identity?.Name);

        var sessionId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAttendanceSummary), new { personId = Guid.Empty }, sessionId);
    }

    [HttpPost("mark")]
    [Authorize(Roles = "Admin,AcademicAdmin,Teacher")]
    public async Task<ActionResult> MarkAttendance(
        [FromBody] MarkAttendanceRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new MarkAttendanceCommand(
            request.SessionId,
            request.PersonId,
            request.PersonType,
            request.Status,
            User?.Identity?.Name,
            request.DeviceId,
            request.DeviceType,
            request.Remarks);

        var result = await mediator.Send(command, cancellationToken);
        return result ? Ok(new { message = "Attendance marked successfully." }) : BadRequest(new { message = "Failed to mark attendance." });
    }

    [HttpPost("mark-bulk")]
    [Authorize(Roles = "Admin,AcademicAdmin,Teacher")]
    public async Task<ActionResult> MarkBulkAttendance(
        [FromBody] MarkBulkAttendanceRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new MarkBulkAttendanceCommand(
            request.SessionId,
            request.Items.Select(i => new Application.Attendance.Interfaces.BulkAttendanceItem(
                i.PersonId,
                i.Status,
                i.Remarks)).ToList(),
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        return result ? Ok(new { message = "Bulk attendance marked successfully." }) : BadRequest(new { message = "Failed to mark bulk attendance." });
    }

    [HttpGet("summary/{personId:guid}")]
    [Authorize(Roles = "Admin,AcademicAdmin,Teacher,Student,Parent")]
    public async Task<ActionResult<AttendanceSummaryDto>> GetAttendanceSummary(
        Guid personId,
        [FromQuery] PersonType personType,
        [FromQuery] string? academicYear = null,
        [FromQuery] string? term = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAttendanceSummaryQuery(
            personId,
            personType,
            academicYear,
            term,
            fromDate,
            toDate);

        var result = await mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("absentees")]
    [Authorize(Roles = "Admin,AcademicAdmin,Teacher")]
    public async Task<ActionResult<IReadOnlyCollection<AbsenteeAlertDto>>> GetAbsentees(
        [FromQuery] DateTime date,
        [FromQuery] Guid? classSectionId = null,
        [FromQuery] Guid? courseId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAbsenteesQuery(date, classSectionId, courseId);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}

// Request DTOs
public sealed record CreateAttendanceSessionRequest(
    string SessionName,
    SessionType Type,
    DateTime SessionDate,
    string AcademicYear,
    Guid? ClassSectionId = null,
    Guid? CourseId = null,
    Guid? StaffShiftId = null,
    TimeOnly? StartTime = null,
    TimeOnly? EndTime = null,
    string? Term = null,
    string? Remarks = null);

public sealed record MarkAttendanceRequest(
    Guid SessionId,
    Guid PersonId,
    PersonType PersonType,
    AttendanceStatus Status,
    string? DeviceId = null,
    string? DeviceType = null,
    string? Remarks = null);

public sealed record MarkBulkAttendanceRequest(
    Guid SessionId,
    IReadOnlyCollection<BulkAttendanceItemRequest> Items);

public sealed record BulkAttendanceItemRequest(
    Guid PersonId,
    AttendanceStatus Status,
    string? Remarks = null);

