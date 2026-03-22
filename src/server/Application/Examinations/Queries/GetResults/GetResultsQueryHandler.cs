using ERP.Application.Admissions.Interfaces;
using ERP.Application.Academics.Interfaces;
using ERP.Application.Examinations.Interfaces;
using ERP.Application.Students.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Examinations.Queries.GetResults;

public class GetResultsQueryHandler : IRequestHandler<GetResultsQuery, ResultSummaryDto?>
{
    private readonly IResultSummaryRepository _resultSummaryRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IAcademicTermRepository _academicTermRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<GetResultsQueryHandler> _logger;

    public GetResultsQueryHandler(
        IResultSummaryRepository resultSummaryRepository,
        IStudentRepository studentRepository,
        IAcademicTermRepository academicTermRepository,
        ICourseRepository courseRepository,
        ILogger<GetResultsQueryHandler> logger)
    {
        _resultSummaryRepository = resultSummaryRepository;
        _studentRepository = studentRepository;
        _academicTermRepository = academicTermRepository;
        _courseRepository = courseRepository;
        _logger = logger;
    }

    public async Task<ResultSummaryDto?> Handle(GetResultsQuery request, CancellationToken cancellationToken)
    {
        var resultSummary = await _resultSummaryRepository.GetByStudentAndTermAsync(
            request.StudentId,
            request.AcademicTermId,
            cancellationToken);

        if (resultSummary == null)
        {
            return null;
        }

        // Get student
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            _logger.LogWarning("Student {StudentId} not found", request.StudentId);
            return null;
        }

        // Get academic term
        var term = await _academicTermRepository.GetByIdAsync(request.AcademicTermId, cancellationToken);

        // Get course results
        var courseResults = new List<CourseResultDto>();
        foreach (var cr in resultSummary.CourseResults)
        {
            var course = await _courseRepository.GetByIdAsync(cr.CourseId, cancellationToken);
            courseResults.Add(new CourseResultDto(
                cr.Id,
                cr.CourseId,
                course?.Name ?? "Unknown",
                cr.AssessmentId,
                cr.TotalMarks,
                cr.MaxMarks,
                cr.Percentage,
                cr.Grade,
                cr.GradePoints,
                cr.Credits,
                cr.IsPassed));
        }

        return new ResultSummaryDto(
            resultSummary.Id,
            resultSummary.StudentId,
            student.StudentNumber,
            student.FullName,
            resultSummary.AcademicTermId,
            term != null ? $"{term.AcademicYear} - {term.TermName}" : "Unknown",
            resultSummary.TotalMarks,
            resultSummary.MaxMarks,
            resultSummary.Percentage,
            resultSummary.Grade,
            resultSummary.GPA,
            resultSummary.TotalCredits,
            resultSummary.EarnedCredits,
            resultSummary.Status.ToString(),
            resultSummary.IsPublished,
            resultSummary.PublishedOnUtc,
            resultSummary.PublishedBy,
            courseResults);
    }
}

