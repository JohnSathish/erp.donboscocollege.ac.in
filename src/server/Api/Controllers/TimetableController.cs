using ERP.Application.Academics.Commands.CreateAcademicTerm;
using ERP.Application.Academics.Commands.ActivateAcademicTerm;
using ERP.Application.Academics.Commands.DeactivateAcademicTerm;
using ERP.Application.Academics.Commands.CreateClassSection;
using ERP.Application.Academics.Commands.CreateTimetableSlot;
using ERP.Application.Academics.Commands.CreateRoom;
using ERP.Application.Academics.Queries.GetTimetable;
using ERP.Application.Academics.Queries.CheckConflicts;
using ERP.Application.Academics.Queries.GetAvailableRooms;
using ERP.Application.Academics.Queries.ListAcademicTerms;
using ERP.Application.Academics.Interfaces;
using ERP.Domain.Academics.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TimetableController(IMediator mediator) : ControllerBase
{
    [HttpPost("terms")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult<Guid>> CreateAcademicTerm(
        [FromBody] CreateAcademicTermRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateAcademicTermCommand(
            request.TermName,
            request.AcademicYear,
            request.StartDate,
            request.EndDate,
            request.Remarks,
            User?.Identity?.Name);

        var termId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetTimetable), new { }, termId);
    }

    [HttpGet("terms")]
    [Authorize(Roles = "Admin,AcademicAdmin,Teacher,Student,Parent")]
    public async Task<ActionResult<IReadOnlyCollection<AcademicTermDto>>> ListAcademicTerms(
        [FromQuery] string? academicYear = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListAcademicTermsQuery(academicYear, isActive);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("terms/{id:guid}/activate")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult> ActivateAcademicTerm(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new ActivateAcademicTermCommand(id, User?.Identity?.Name);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("terms/{id:guid}/deactivate")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult> DeactivateAcademicTerm(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeactivateAcademicTermCommand(id, User?.Identity?.Name);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("sections")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult<Guid>> CreateClassSection(
        [FromBody] CreateClassSectionRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateClassSectionCommand(
            request.SectionName,
            request.CourseId,
            request.AcademicYear,
            request.Shift,
            request.Capacity,
            request.TermId,
            request.TeacherId,
            request.TeacherName,
            request.RoomNumber,
            request.Building,
            request.Remarks,
            User?.Identity?.Name);

        var sectionId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetTimetable), new { }, sectionId);
    }

    [HttpPost("slots")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult<CreateTimetableSlotResult>> CreateTimetableSlot(
        [FromBody] CreateTimetableSlotRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateTimetableSlotCommand(
            request.ClassSectionId,
            request.DayOfWeek,
            request.StartTime,
            request.EndTime,
            request.TeacherId,
            request.TeacherName,
            request.RoomNumber,
            request.Building,
            request.Remarks,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetTimetable), new { classSectionId = request.ClassSectionId }, result);
    }

    [HttpPost("rooms")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult<Guid>> CreateRoom(
        [FromBody] CreateRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateRoomCommand(
            request.RoomNumber,
            request.Type,
            request.Capacity,
            request.Building,
            request.Floor,
            request.HasProjector,
            request.HasComputerLab,
            request.HasWhiteboard,
            request.Equipment,
            request.Remarks,
            User?.Identity?.Name);

        try
        {
            var roomId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetTimetable), new { }, roomId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("timetable")]
    [Authorize(Roles = "Admin,AcademicAdmin,Teacher,Student,Parent")]
    public async Task<ActionResult<IReadOnlyCollection<TimetableSlotDto>>> GetTimetable(
        [FromQuery] Guid? classSectionId = null,
        [FromQuery] Guid? teacherId = null,
        [FromQuery] string? roomNumber = null,
        [FromQuery] string? building = null,
        [FromQuery] DayOfWeek? dayOfWeek = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTimetableQuery(classSectionId, teacherId, roomNumber, building, dayOfWeek);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("check-conflicts")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult<ConflictCheckResult>> CheckConflicts(
        [FromBody] CheckConflictsRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = new CheckConflictsQuery(
            request.DayOfWeek,
            request.StartTime,
            request.EndTime,
            request.TeacherId,
            request.RoomNumber,
            request.Building,
            request.ExcludeSlotId);

        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("available-rooms")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult<IReadOnlyCollection<RoomAvailabilityDto>>> GetAvailableRooms(
        [FromQuery] DayOfWeek dayOfWeek,
        [FromQuery] TimeOnly startTime,
        [FromQuery] TimeOnly endTime,
        [FromQuery] int minCapacity = 1,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAvailableRoomsQuery(dayOfWeek, startTime, endTime, minCapacity);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}

// Request DTOs
public sealed record CreateAcademicTermRequest(
    string TermName,
    string AcademicYear,
    DateTime StartDate,
    DateTime EndDate,
    string? Remarks = null);

public sealed record CreateClassSectionRequest(
    string SectionName,
    Guid CourseId,
    string AcademicYear,
    string Shift,
    int Capacity,
    Guid? TermId = null,
    Guid? TeacherId = null,
    string? TeacherName = null,
    string? RoomNumber = null,
    string? Building = null,
    string? Remarks = null);

public sealed record CreateTimetableSlotRequest(
    Guid ClassSectionId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    Guid? TeacherId = null,
    string? TeacherName = null,
    string? RoomNumber = null,
    string? Building = null,
    string? Remarks = null);

public sealed record CreateRoomRequest(
    string RoomNumber,
    RoomType Type,
    int Capacity,
    string? Building = null,
    string? Floor = null,
    bool HasProjector = false,
    bool HasComputerLab = false,
    bool HasWhiteboard = true,
    string? Equipment = null,
    string? Remarks = null);

public sealed record CheckConflictsRequest(
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    Guid? TeacherId = null,
    string? RoomNumber = null,
    string? Building = null,
    Guid? ExcludeSlotId = null);

