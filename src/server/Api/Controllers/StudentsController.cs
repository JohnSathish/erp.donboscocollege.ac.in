using ERP.Application.Students.Commands.ConvertApplicantToStudent;
using ERP.Application.Students.Commands.UpdateStudentProfile;
using ERP.Application.Students.Commands.UpdateStudentStatus;
using ERP.Application.Students.Commands.CreateAcademicRecord;
using ERP.Application.Students.Commands.EnrollStudentInCourse;
using ERP.Application.Students.Commands.UpdateCourseEnrollmentMarks;
using ERP.Application.Students.Commands.BulkUpdateCourseEnrollmentMarks;
using ERP.Application.Students.Commands.RecalculateAcademicRecordGPA;
using ERP.Application.Students.Commands.ChangeStudentShift;
using ERP.Application.Students.Commands.TransferStudent;
using ERP.Application.Students.Commands.WithdrawStudent;
using ERP.Application.Students.Commands.ApproveStudentTransfer;
using ERP.Application.Students.Commands.ApproveStudentExit;
using ERP.Application.Students.Commands.CreateDisciplineRecord;
using ERP.Application.Students.Commands.CreateCounselingRecord;
using ERP.Application.Students.Queries.GetStudentDashboard;
using ERP.Application.Students.Interfaces;
using StudentMarkEntry = ERP.Application.Students.Commands.BulkUpdateCourseEnrollmentMarks.StudentMarkEntry;
using ERP.Application.Students.Queries.GetStudentById;
using ERP.Application.Students.Queries.GetStudentWithGuardians;
using ERP.Application.Students.Queries.GetStudentAcademicHistory;
using ERP.Application.Students.Queries.ListStudents;
using ERP.Application.Students.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<StudentsListResponse>> ListStudents(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] bool? isActive = null,
        [FromQuery] Guid? programId = null,
        [FromQuery] string? academicYear = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListStudentsQuery(page, pageSize, isActive, programId, academicYear, searchTerm);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StudentDto>> GetStudentById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetStudentByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:guid}/with-guardians")]
    public async Task<ActionResult<StudentWithGuardiansDto>> GetStudentWithGuardians(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetStudentWithGuardiansQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPut("{id:guid}/profile")]
    public async Task<ActionResult<UpdateStudentProfileResult>> UpdateStudentProfile(
        Guid id,
        [FromBody] UpdateStudentProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateStudentProfileCommand(
            id,
            request.FullName,
            request.Email,
            request.MobileNumber,
            request.Shift,
            request.ProgramId,
            request.ProgramCode,
            request.MajorSubject,
            request.MinorSubject,
            request.PhotoUrl,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<UpdateStudentStatusResult>> UpdateStudentStatus(
        Guid id,
        [FromBody] UpdateStudentStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateStudentStatusCommand(
            id,
            request.Status,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("{id:guid}/academic-history")]
    public async Task<ActionResult<StudentAcademicHistoryDto>> GetStudentAcademicHistory(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetStudentAcademicHistoryQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost("{id:guid}/academic-records")]
    public async Task<ActionResult<CreateAcademicRecordResult>> CreateAcademicRecord(
        Guid id,
        [FromBody] CreateAcademicRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateAcademicRecordCommand(
            id,
            request.AcademicYear,
            request.Semester,
            request.TermId,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(
            nameof(GetStudentAcademicHistory),
            new { id },
            result);
    }

    [HttpPost("{id:guid}/course-enrollments")]
    public async Task<ActionResult<EnrollStudentInCourseResult>> EnrollStudentInCourse(
        Guid id,
        [FromBody] EnrollStudentInCourseRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new EnrollStudentInCourseCommand(
            id,
            request.CourseId,
            request.TermId,
            request.EnrollmentType ?? "Regular",
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(
            nameof(GetStudentAcademicHistory),
            new { id },
            result);
    }

    [HttpPut("course-enrollments/{enrollmentId:guid}/marks")]
    public async Task<ActionResult<UpdateCourseEnrollmentMarksResult>> UpdateCourseEnrollmentMarks(
        Guid enrollmentId,
        [FromBody] UpdateCourseEnrollmentMarksRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateCourseEnrollmentMarksCommand(
            enrollmentId,
            request.MarksObtained,
            request.MaxMarks,
            request.Grade,
            request.ResultStatus,
            request.Remarks,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("course-enrollments/bulk-marks")]
    public async Task<ActionResult<BulkUpdateCourseEnrollmentMarksResult>> BulkUpdateCourseEnrollmentMarks(
        [FromBody] BulkUpdateCourseEnrollmentMarksRequest request,
        CancellationToken cancellationToken = default)
    {
        var studentMarks = request.StudentMarks.Select(m => new StudentMarkEntry(
            m.StudentId,
            m.MarksObtained,
            m.Grade,
            m.ResultStatus,
            m.Remarks)).ToList();

        var command = new BulkUpdateCourseEnrollmentMarksCommand(
            request.CourseId,
            request.TermId,
            studentMarks,
            request.MaxMarks,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("academic-records/{recordId:guid}/recalculate-gpa")]
    public async Task<ActionResult<RecalculateAcademicRecordGPAResult>> RecalculateAcademicRecordGPA(
        Guid recordId,
        CancellationToken cancellationToken = default)
    {
        var command = new RecalculateAcademicRecordGPACommand(
            recordId,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("{id:guid}/dashboard")]
    public async Task<ActionResult<StudentDashboardDto>> GetStudentDashboard(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetStudentDashboardQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:guid}/transcript")]
    public async Task<ActionResult> GetStudentTranscript(
        Guid id,
        [FromServices] ITranscriptService transcriptService,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var transcript = await transcriptService.GenerateTranscriptAsync(id, cancellationToken);
            return Ok(transcript);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}/transcript/pdf")]
    public async Task<ActionResult> GetStudentTranscriptPdf(
        Guid id,
        [FromServices] ITranscriptService transcriptService,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var pdfBytes = await transcriptService.GenerateTranscriptPdfAsync(id, cancellationToken);
            return File(pdfBytes, "application/pdf", $"transcript-{id}.pdf");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("{id:guid}/academic-records/{recordId:guid}/evaluate-progression")]
    public async Task<ActionResult<ProgressionEvaluationResult>> EvaluateProgression(
        Guid id,
        Guid recordId,
        [FromServices] IAcademicProgressionService progressionService,
        CancellationToken cancellationToken = default)
    {
        var result = await progressionService.EvaluateProgressionAsync(id, recordId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/graduation-eligibility")]
    public async Task<ActionResult<GraduationEligibilityResult>> CheckGraduationEligibility(
        Guid id,
        [FromServices] IAcademicProgressionService progressionService,
        CancellationToken cancellationToken = default)
    {
        var result = await progressionService.CheckGraduationEligibilityAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost("convert-from-applicant")]
    public async Task<ActionResult<Guid>> ConvertApplicantToStudent(
        [FromBody] ConvertApplicantToStudentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ConvertApplicantToStudentCommand(
            request.ApplicantAccountId,
            request.StudentNumber,
            request.AcademicYear,
            request.ProgramId,
            request.ProgramCode,
            User?.Identity?.Name);

        try
        {
            var studentId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetStudentById), new { id = studentId }, studentId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}/shift")]
    [Authorize(Roles = "Admin,AcademicAdmin,Student")]
    public async Task<ActionResult<ChangeStudentShiftResult>> ChangeStudentShift(
        Guid id,
        [FromBody] ChangeStudentShiftRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new ChangeStudentShiftCommand(
            id,
            request.NewShift,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("{id:guid}/transfer")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult<TransferStudentResult>> TransferStudent(
        Guid id,
        [FromBody] TransferStudentRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new TransferStudentCommand(
            id,
            request.Reason,
            request.EffectiveDate,
            request.ToProgramId,
            request.ToProgramCode,
            request.ToShift,
            request.ToSection,
            User?.Identity?.Name,
            request.Remarks);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetStudentById), new { id = id }, result);
    }

    [HttpPost("{id:guid}/withdraw")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult<WithdrawStudentResult>> WithdrawStudent(
        Guid id,
        [FromBody] WithdrawStudentRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new WithdrawStudentCommand(
            id,
            request.ExitType,
            request.Reason,
            request.EffectiveDate,
            User?.Identity?.Name,
            request.Remarks);

        var result = await mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetStudentById), new { id = id }, result);
    }

    [HttpPost("transfers/{transferId:guid}/approve")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult> ApproveStudentTransfer(
        Guid transferId,
        [FromBody] ApproveTransferRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        var command = new ApproveStudentTransferCommand(
            transferId,
            request?.Remarks,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("exits/{exitId:guid}/approve")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult> ApproveStudentExit(
        Guid exitId,
        [FromBody] ApproveExitRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        var command = new ApproveStudentExitCommand(
            exitId,
            request?.Remarks,
            User?.Identity?.Name);

        var result = await mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/discipline-records")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult<Guid>> CreateDisciplineRecord(
        Guid id,
        [FromBody] CreateDisciplineRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateDisciplineRecordCommand(
            id,
            request.IncidentType,
            request.Description,
            request.IncidentDate,
            request.Severity,
            request.Location,
            request.ReportedBy,
            request.Witnesses,
            User?.Identity?.Name);

        try
        {
            var recordId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetStudentById), new { id = id }, recordId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id:guid}/counseling-records")]
    [Authorize(Roles = "Admin,AcademicAdmin")]
    public async Task<ActionResult<Guid>> CreateCounselingRecord(
        Guid id,
        [FromBody] CreateCounselingRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateCounselingRecordCommand(
            id,
            request.SessionType,
            request.SessionDate,
            request.CounselorName,
            request.CounselorId,
            request.Location,
            User?.Identity?.Name);

        try
        {
            var recordId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetStudentById), new { id = id }, recordId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public sealed record UpdateStudentProfileRequest(
    string FullName,
    string Email,
    string MobileNumber,
    string Shift,
    Guid? ProgramId = null,
    string? ProgramCode = null,
    string? MajorSubject = null,
    string? MinorSubject = null,
    string? PhotoUrl = null);

public sealed record UpdateStudentStatusRequest(string Status);

public sealed record CreateAcademicRecordRequest(
    string AcademicYear,
    string Semester,
    Guid? TermId = null);

public sealed record EnrollStudentInCourseRequest(
    Guid CourseId,
    Guid? TermId = null,
    string? EnrollmentType = null);

public sealed record UpdateCourseEnrollmentMarksRequest(
    decimal? MarksObtained,
    decimal? MaxMarks,
    string? Grade = null,
    string? ResultStatus = null,
    string? Remarks = null);

public sealed record BulkUpdateCourseEnrollmentMarksRequest(
    Guid CourseId,
    Guid? TermId,
    decimal MaxMarks,
    IReadOnlyCollection<StudentMarkEntryRequest> StudentMarks);

public sealed record StudentMarkEntryRequest(
    Guid StudentId,
    decimal MarksObtained,
    string? Grade = null,
    string? ResultStatus = null,
    string? Remarks = null);

public sealed record ConvertApplicantToStudentRequest(
    Guid ApplicantAccountId,
    string StudentNumber,
    string AcademicYear,
    Guid? ProgramId = null,
    string? ProgramCode = null);

public sealed record ChangeStudentShiftRequest(
    string NewShift);

public sealed record TransferStudentRequest(
    Guid? ToProgramId = null,
    string? ToProgramCode = null,
    string? ToShift = null,
    string? ToSection = null,
    string Reason = "",
    DateTime EffectiveDate = default,
    string? Remarks = null);

public sealed record WithdrawStudentRequest(
    ERP.Domain.Students.Entities.ExitType ExitType,
    string Reason = "",
    DateTime? EffectiveDate = null,
    string? Remarks = null);

public sealed record ApproveTransferRequest(
    string? Remarks = null);

public sealed record ApproveExitRequest(
    string? Remarks = null);

public sealed record CreateDisciplineRecordRequest(
    string IncidentType,
    string Description,
    DateTime IncidentDate,
    ERP.Domain.Students.Entities.DisciplineSeverity Severity,
    string? Location = null,
    string? ReportedBy = null,
    string? Witnesses = null);

public sealed record CreateCounselingRecordRequest(
    string SessionType,
    DateTime SessionDate,
    string? CounselorName = null,
    string? CounselorId = null,
    string? Location = null);


