using ERP.Application.Examinations.Commands.ApproveMarks;
using ERP.Application.Examinations.Commands.BulkEnterMarks;
using ERP.Application.Examinations.Commands.CreateAssessment;
using ERP.Application.Examinations.Commands.EnterMarks;
using ERP.Application.Examinations.Commands.ProcessResults;
using ERP.Application.Examinations.Commands.PublishAssessment;
using ERP.Application.Examinations.Queries.GetAssessment;
using ERP.Application.Examinations.Queries.GetResults;
using ERP.Application.Examinations.Queries.GetStudentMarks;
using ERP.Application.Examinations.Queries.ListAssessments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExaminationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ExaminationsController> _logger;

    public ExaminationsController(IMediator mediator, ILogger<ExaminationsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("assessments")]
    public async Task<ActionResult<Guid>> CreateAssessment(
        [FromBody] CreateAssessmentRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var components = request.Components?.Select(c => new ERP.Application.Examinations.Commands.CreateAssessment.AssessmentComponentDto(
                c.Name,
                c.MaxMarks,
                c.PassingMarks,
                c.Weightage,
                c.DisplayOrder,
                c.Code,
                c.Instructions)).ToList();

            var command = new CreateAssessmentCommand(
                request.CourseId,
                request.AcademicTermId,
                request.Name,
                request.Code,
                request.Type,
                request.MaxMarks,
                request.PassingMarks,
                request.TotalWeightage,
                request.ClassSectionId,
                request.ScheduledDate,
                request.Duration,
                request.Instructions,
                components,
                request.CreatedBy);

            var assessmentId = await _mediator.Send(command, cancellationToken);
            return Ok(assessmentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating assessment");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("assessments/{id}")]
    public async Task<ActionResult<AssessmentDto>> GetAssessment(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetAssessmentQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("assessments")]
    public async Task<ActionResult<IReadOnlyCollection<AssessmentSummaryDto>>> ListAssessments(
        [FromQuery] Guid? courseId,
        [FromQuery] Guid? academicTermId,
        [FromQuery] Guid? classSectionId,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var query = new ListAssessmentsQuery(courseId, academicTermId, classSectionId, status);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("assessments/{id}/publish")]
    public async Task<ActionResult> PublishAssessment(
        Guid id,
        [FromBody] PublishAssessmentRequest? request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new PublishAssessmentCommand(id, request?.PublishedBy);
            await _mediator.Send(command, cancellationToken);
            return Ok(new { message = "Assessment published successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing assessment {AssessmentId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("marks")]
    public async Task<ActionResult<Guid>> EnterMarks(
        [FromBody] EnterMarksRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new EnterMarksCommand(
                request.AssessmentComponentId,
                request.StudentId,
                request.MarksObtained,
                request.IsAbsent,
                request.IsExempted,
                request.Remarks,
                request.EnteredBy);

            var markEntryId = await _mediator.Send(command, cancellationToken);
            return Ok(markEntryId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error entering marks");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("marks/bulk")]
    public async Task<ActionResult<int>> BulkEnterMarks(
        [FromBody] BulkEnterMarksRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var studentMarks = request.StudentMarks.Select(sm => new StudentMarkDto(
                sm.StudentId,
                sm.MarksObtained,
                sm.IsAbsent,
                sm.IsExempted,
                sm.Remarks)).ToList();

            var command = new BulkEnterMarksCommand(
                request.AssessmentComponentId,
                studentMarks,
                request.EnteredBy);

            var count = await _mediator.Send(command, cancellationToken);
            return Ok(new { count, message = $"Successfully entered marks for {count} students" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk entering marks");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("marks/student/{studentId}")]
    public async Task<ActionResult<IReadOnlyCollection<MarkEntryDto>>> GetStudentMarks(
        Guid studentId,
        [FromQuery] Guid? academicTermId,
        CancellationToken cancellationToken)
    {
        var query = new GetStudentMarksQuery(studentId, academicTermId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("marks/{id}/approve")]
    public async Task<ActionResult> ApproveMarks(
        Guid id,
        [FromBody] ApproveMarksRequest? request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new ApproveMarksCommand(id, request?.ApprovedBy);
            await _mediator.Send(command, cancellationToken);
            return Ok(new { message = "Marks approved successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving marks {MarkEntryId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("results/process")]
    public async Task<ActionResult<Guid>> ProcessResults(
        [FromBody] ProcessResultsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new ProcessResultsCommand(
                request.StudentId,
                request.AcademicTermId,
                request.ProcessedBy);

            var resultSummaryId = await _mediator.Send(command, cancellationToken);
            return Ok(new { resultSummaryId, message = "Results processed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing results");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("results/student/{studentId}/term/{academicTermId}")]
    public async Task<ActionResult<ResultSummaryDto>> GetResults(
        Guid studentId,
        Guid academicTermId,
        CancellationToken cancellationToken)
    {
        var query = new GetResultsQuery(studentId, academicTermId);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}

// Request DTOs
public record CreateAssessmentRequest(
    Guid CourseId,
    Guid AcademicTermId,
    string Name,
    string Code,
    string Type,
    int MaxMarks,
    int PassingMarks,
    decimal TotalWeightage,
    Guid? ClassSectionId = null,
    DateTime? ScheduledDate = null,
    TimeSpan? Duration = null,
    string? Instructions = null,
    List<AssessmentComponentRequest>? Components = null,
    string? CreatedBy = null);

public record AssessmentComponentRequest(
    string Name,
    int MaxMarks,
    int PassingMarks,
    decimal Weightage,
    int DisplayOrder,
    string? Code = null,
    string? Instructions = null);

public record PublishAssessmentRequest(string? PublishedBy = null);

public record EnterMarksRequest(
    Guid AssessmentComponentId,
    Guid StudentId,
    decimal MarksObtained,
    bool IsAbsent = false,
    bool IsExempted = false,
    string? Remarks = null,
    string? EnteredBy = null);

public record BulkEnterMarksRequest(
    Guid AssessmentComponentId,
    List<StudentMarkRequest> StudentMarks,
    string? EnteredBy = null);

public record StudentMarkRequest(
    Guid StudentId,
    decimal MarksObtained,
    bool IsAbsent = false,
    bool IsExempted = false,
    string? Remarks = null);

public record ApproveMarksRequest(string? ApprovedBy = null);

public record ProcessResultsRequest(
    Guid StudentId,
    Guid AcademicTermId,
    string? ProcessedBy = null);

