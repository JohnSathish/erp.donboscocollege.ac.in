using ERP.Application.Admissions.Commands.CreateProgram;
using ERP.Application.Admissions.Commands.UpdateProgram;
using ERP.Application.Admissions.Commands.DeleteProgram;
using ERP.Application.Admissions.Commands.ToggleProgramStatus;
using ERP.Application.Admissions.Commands.CreateCourse;
using ERP.Application.Admissions.Commands.UpdateCourse;
using ERP.Application.Admissions.Commands.DeleteCourse;
using ERP.Application.Admissions.Commands.ToggleCourseStatus;
using ERP.Application.Admissions.Commands.CreateFeeStructure;
using ERP.Application.Admissions.Commands.UpdateFeeStructure;
using ERP.Application.Admissions.Commands.DeleteFeeStructure;
using ERP.Application.Admissions.Commands.ToggleFeeStructureStatus;
using ERP.Application.Admissions.Commands.AddFeeComponent;
using ERP.Application.Admissions.Commands.UpdateFeeComponent;
using ERP.Application.Admissions.Commands.DeleteFeeComponent;
using ERP.Application.Admissions.Queries.ListPrograms;
using ERP.Application.Admissions.Queries.GetProgramById;
using ERP.Application.Admissions.Queries.ListCourses;
using ERP.Application.Admissions.Queries.GetCourseById;
using ERP.Application.Admissions.Queries.ListFeeStructures;
using ERP.Application.Admissions.Queries.GetFeeStructureById;
using ERP.Application.Admissions.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SettingsController(IMediator mediator) : ControllerBase
{
    // ========== Programs ==========
    [HttpGet("programs")]
    public async Task<ActionResult<ProgramsListResponse>> ListPrograms(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListProgramsQuery(page, pageSize, isActive, searchTerm);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("programs/{id:guid}")]
    public async Task<ActionResult<ProgramDto>> GetProgramById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProgramByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost("programs")]
    public async Task<ActionResult<Guid>> CreateProgram(
        [FromBody] CreateProgramRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProgramCommand(
            request.Code,
            request.Name,
            request.Level,
            request.DurationYears,
            request.TotalCredits,
            request.Description,
            User?.Identity?.Name);

        try
        {
            var programId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetProgramById), new { id = programId }, programId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("programs/{id:guid}")]
    public async Task<IActionResult> UpdateProgram(
        Guid id,
        [FromBody] UpdateProgramRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProgramCommand(
            id,
            request.Name,
            request.Level,
            request.DurationYears,
            request.TotalCredits,
            request.Description,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("programs/{id:guid}")]
    public async Task<IActionResult> DeleteProgram(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProgramCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPatch("programs/{id:guid}/status")]
    public async Task<IActionResult> ToggleProgramStatus(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new ToggleProgramStatusCommand(id, User?.Identity?.Name);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    // ========== Courses ==========
    [HttpGet("courses")]
    public async Task<ActionResult<CoursesListResponse>> ListCourses(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] bool? isActive = null,
        [FromQuery] Guid? programId = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListCoursesQuery(page, pageSize, isActive, programId, searchTerm);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("courses/{id:guid}")]
    public async Task<ActionResult<CourseDto>> GetCourseById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCourseByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost("courses")]
    public async Task<ActionResult<Guid>> CreateCourse(
        [FromBody] CreateCourseRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCourseCommand(
            request.Code,
            request.Name,
            request.CreditHours,
            request.ProgramId,
            request.Description,
            request.Prerequisites,
            User?.Identity?.Name);

        try
        {
            var courseId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetCourseById), new { id = courseId }, courseId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("courses/{id:guid}")]
    public async Task<IActionResult> UpdateCourse(
        Guid id,
        [FromBody] UpdateCourseRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCourseCommand(
            id,
            request.Name,
            request.CreditHours,
            request.ProgramId,
            request.Description,
            request.Prerequisites,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("courses/{id:guid}")]
    public async Task<IActionResult> DeleteCourse(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCourseCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPatch("courses/{id:guid}/status")]
    public async Task<IActionResult> ToggleCourseStatus(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new ToggleCourseStatusCommand(id, User?.Identity?.Name);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    // ========== Fee Structures ==========
    [HttpGet("fee-structures")]
    public async Task<ActionResult<FeeStructuresListResponse>> ListFeeStructures(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] bool? isActive = null,
        [FromQuery] Guid? programId = null,
        [FromQuery] string? academicYear = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListFeeStructuresQuery(page, pageSize, isActive, programId, academicYear, searchTerm);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("fee-structures/{id:guid}")]
    public async Task<ActionResult<FeeStructureDto>> GetFeeStructureById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetFeeStructureByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost("fee-structures")]
    public async Task<ActionResult<Guid>> CreateFeeStructure(
        [FromBody] CreateFeeStructureRequest request,
        CancellationToken cancellationToken)
    {
        var components = request.Components?.Select(c => new FeeComponentRequest(
            c.Name,
            c.Amount,
            c.IsOptional,
            c.InstallmentNumber,
            c.DueDateUtc,
            c.Description,
            c.DisplayOrder)).ToList();

        var command = new CreateFeeStructureCommand(
            request.Name,
            request.AcademicYear,
            request.EffectiveFromUtc,
            request.ProgramId,
            request.Description,
            request.EffectiveToUtc,
            components,
            User?.Identity?.Name);

        try
        {
            var feeStructureId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetFeeStructureById), new { id = feeStructureId }, feeStructureId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("fee-structures/{id:guid}")]
    public async Task<IActionResult> UpdateFeeStructure(
        Guid id,
        [FromBody] UpdateFeeStructureRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateFeeStructureCommand(
            id,
            request.Name,
            request.AcademicYear,
            request.EffectiveFromUtc,
            request.ProgramId,
            request.Description,
            request.EffectiveToUtc,
            User?.Identity?.Name);

        try
        {
            var result = await mediator.Send(command, cancellationToken);
            return result ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("fee-structures/{id:guid}")]
    public async Task<IActionResult> DeleteFeeStructure(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFeeStructureCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPatch("fee-structures/{id:guid}/status")]
    public async Task<IActionResult> ToggleFeeStructureStatus(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new ToggleFeeStructureStatusCommand(id, User?.Identity?.Name);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    // ========== Fee Components ==========
    [HttpPost("fee-structures/{feeStructureId:guid}/components")]
    public async Task<ActionResult<Guid>> AddFeeComponent(
        Guid feeStructureId,
        [FromBody] AddFeeComponentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddFeeComponentCommand(
            feeStructureId,
            request.Name,
            request.Amount,
            request.IsOptional,
            request.InstallmentNumber,
            request.DueDateUtc,
            request.Description,
            request.DisplayOrder,
            User?.Identity?.Name);

        try
        {
            var componentId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetFeeStructureById), new { id = feeStructureId }, componentId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("fee-components/{id:guid}")]
    public async Task<IActionResult> UpdateFeeComponent(
        Guid id,
        [FromBody] UpdateFeeComponentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateFeeComponentCommand(
            id,
            request.Name,
            request.Amount,
            request.IsOptional,
            request.InstallmentNumber,
            request.DueDateUtc,
            request.Description,
            request.DisplayOrder,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("fee-components/{id:guid}")]
    public async Task<IActionResult> DeleteFeeComponent(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFeeComponentCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }
}

// Request Models
public sealed record CreateProgramRequest(
    string Code,
    string Name,
    string Level,
    int DurationYears,
    int TotalCredits,
    string? Description = null);

public sealed record UpdateProgramRequest(
    string Name,
    string Level,
    int DurationYears,
    int TotalCredits,
    string? Description = null);

public sealed record CreateCourseRequest(
    string Code,
    string Name,
    int CreditHours,
    Guid? ProgramId = null,
    string? Description = null,
    string? Prerequisites = null);

public sealed record UpdateCourseRequest(
    string Name,
    int CreditHours,
    Guid? ProgramId = null,
    string? Description = null,
    string? Prerequisites = null);

public sealed record CreateFeeStructureRequest(
    string Name,
    string AcademicYear,
    DateTime EffectiveFromUtc,
    Guid? ProgramId = null,
    string? Description = null,
    DateTime? EffectiveToUtc = null,
    IReadOnlyCollection<FeeComponentRequestDto>? Components = null);

public sealed record FeeComponentRequestDto(
    string Name,
    decimal Amount,
    bool IsOptional = false,
    int? InstallmentNumber = null,
    DateTime? DueDateUtc = null,
    string? Description = null,
    int DisplayOrder = 0);

public sealed record UpdateFeeStructureRequest(
    string Name,
    string AcademicYear,
    DateTime EffectiveFromUtc,
    Guid? ProgramId = null,
    string? Description = null,
    DateTime? EffectiveToUtc = null);

public sealed record AddFeeComponentRequest(
    string Name,
    decimal Amount,
    bool IsOptional = false,
    int? InstallmentNumber = null,
    DateTime? DueDateUtc = null,
    string? Description = null,
    int DisplayOrder = 0);

public sealed record UpdateFeeComponentRequest(
    string Name,
    decimal Amount,
    bool IsOptional = false,
    int? InstallmentNumber = null,
    DateTime? DueDateUtc = null,
    string? Description = null,
    int DisplayOrder = 0);


