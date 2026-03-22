using ERP.Application.Examinations.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Examinations.Entities;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Examinations;

public class ResultProcessingService : IResultProcessingService
{
    private readonly IMarkEntryRepository _markEntryRepository;
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly IResultSummaryRepository _resultSummaryRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IAcademicHistoryRepository _academicHistoryRepository;
    private readonly IGradingService _gradingService;
    private readonly ILogger<ResultProcessingService> _logger;

    public ResultProcessingService(
        IMarkEntryRepository markEntryRepository,
        IAssessmentRepository assessmentRepository,
        IResultSummaryRepository resultSummaryRepository,
        IStudentRepository studentRepository,
        IAcademicHistoryRepository academicHistoryRepository,
        IGradingService gradingService,
        ILogger<ResultProcessingService> logger)
    {
        _markEntryRepository = markEntryRepository;
        _assessmentRepository = assessmentRepository;
        _resultSummaryRepository = resultSummaryRepository;
        _studentRepository = studentRepository;
        _academicHistoryRepository = academicHistoryRepository;
        _gradingService = gradingService;
        _logger = logger;
    }

    public async Task<ResultSummary> ProcessStudentResultsAsync(
        Guid studentId,
        Guid academicTermId,
        CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student == null)
            throw new InvalidOperationException($"Student with ID {studentId} not found.");

        // Get all mark entries for the student in the term
        var markEntries = await _markEntryRepository.GetByStudentIdAsync(studentId, academicTermId, cancellationToken);
        
        if (!markEntries.Any())
        {
            _logger.LogWarning("No mark entries found for student {StudentId} in term {TermId}", studentId, academicTermId);
        }

        // Get course enrollments for the term
        var enrollments = await _academicHistoryRepository.GetCourseEnrollmentsByStudentIdAsync(studentId, academicTermId, cancellationToken);
        
        var courseResults = new List<CourseResult>();
        var totalMarks = 0m;
        var maxMarks = 0m;
        var totalCredits = 0;
        var earnedCredits = 0;

        // Process results for each enrolled course
        foreach (var enrollment in enrollments)
        {
            var courseResult = await CalculateCourseResultAsync(studentId, enrollment.CourseId, academicTermId, cancellationToken);
            if (courseResult != null)
            {
                courseResults.Add(courseResult);
                totalMarks += courseResult.TotalMarks;
                maxMarks += courseResult.MaxMarks;
                totalCredits += courseResult.Credits;
                if (courseResult.IsPassed)
                {
                    earnedCredits += courseResult.Credits;
                }
            }
        }

        // Calculate overall percentage
        var percentage = maxMarks > 0 ? (totalMarks / maxMarks) * 100 : 0;
        var grade = _gradingService.CalculateGrade(percentage);

        // Calculate GPA
        var courseGradeInfos = courseResults.Select(cr => new CourseGradeInfo(
            cr.Percentage,
            cr.Credits,
            cr.GradePoints)).ToList();
        var gpa = _gradingService.CalculateGPA(courseGradeInfos);

        // Get or create result summary
        var existingSummary = await _resultSummaryRepository.GetByStudentAndTermAsync(studentId, academicTermId, cancellationToken);
        
        if (existingSummary != null)
        {
            existingSummary.UpdateResults(
                totalMarks,
                maxMarks,
                percentage,
                grade,
                gpa,
                totalCredits,
                earnedCredits,
                "System");
            
            // Update course results
            // Remove old course results and add new ones
            // Note: This is simplified - in production, you might want to update existing ones
            existingSummary.CourseResults.Clear();
            foreach (var cr in courseResults)
            {
                existingSummary.CourseResults.Add(cr);
            }
            
            await _resultSummaryRepository.UpdateAsync(existingSummary, cancellationToken);
            return existingSummary;
        }
        else
        {
            var resultSummary = new ResultSummary(studentId, academicTermId, "System");
            resultSummary.UpdateResults(
                totalMarks,
                maxMarks,
                percentage,
                grade,
                gpa,
                totalCredits,
                earnedCredits,
                "System");
            
            foreach (var cr in courseResults)
            {
                resultSummary.CourseResults.Add(cr);
            }
            
            return await _resultSummaryRepository.AddAsync(resultSummary, cancellationToken);
        }
    }

    public async Task<IReadOnlyCollection<ResultSummary>> ProcessTermResultsAsync(
        Guid academicTermId,
        CancellationToken cancellationToken = default)
    {
        // Get all assessments for the term
        var assessments = await _assessmentRepository.GetByAcademicTermAsync(academicTermId, cancellationToken);
        
        // Get unique student IDs from mark entries
        var studentIds = new HashSet<Guid>();
        foreach (var assessment in assessments)
        {
            var markEntries = await _markEntryRepository.GetByAssessmentIdAsync(assessment.Id, cancellationToken);
            foreach (var entry in markEntries)
            {
                studentIds.Add(entry.StudentId);
            }
        }

        var results = new List<ResultSummary>();
        foreach (var studentId in studentIds)
        {
            try
            {
                var result = await ProcessStudentResultsAsync(studentId, academicTermId, cancellationToken);
                results.Add(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing results for student {StudentId} in term {TermId}", studentId, academicTermId);
            }
        }

        return results;
    }

    public async Task<CourseResult> CalculateCourseResultAsync(
        Guid studentId,
        Guid courseId,
        Guid academicTermId,
        CancellationToken cancellationToken = default)
    {
        // Get all assessments for this course in the term
        var assessments = await _assessmentRepository.GetByCourseAndTermAsync(courseId, academicTermId, cancellationToken);
        
        // Get all mark entries for this student in these assessments
        var allMarkEntries = new List<MarkEntry>();
        foreach (var assessment in assessments)
        {
            var entries = await _markEntryRepository.GetByAssessmentIdAsync(assessment.Id, cancellationToken);
            var studentEntries = entries.Where(e => e.StudentId == studentId && e.Status == MarkEntryStatus.Approved);
            allMarkEntries.AddRange(studentEntries);
        }

        if (!allMarkEntries.Any())
        {
            _logger.LogWarning("No approved mark entries found for student {StudentId} in course {CourseId}", studentId, courseId);
            return null!; // Return null to indicate no result
        }

        // Calculate total marks and max marks
        var totalMarks = 0m;
        var maxMarks = 0m;

        foreach (var entry in allMarkEntries)
        {
            if (!entry.IsAbsent && !entry.IsExempted)
            {
                totalMarks += entry.MarksObtained;
                maxMarks += entry.AssessmentComponent.MaxMarks;
            }
        }

        // Calculate percentage
        var percentage = maxMarks > 0 ? (totalMarks / maxMarks) * 100 : 0;
        var grade = _gradingService.CalculateGrade(percentage);
        var gradePoints = GetGradePoints(percentage);

        // Get course credits (assuming 3 credits per course - should come from course definition)
        var credits = 3; // TODO: Get from course definition

        // Get primary assessment ID (first assessment)
        var primaryAssessmentId = assessments.FirstOrDefault()?.Id;

        return new CourseResult(
            Guid.Empty, // Will be set when added to ResultSummary
            courseId,
            totalMarks,
            maxMarks,
            percentage,
            credits,
            primaryAssessmentId,
            grade,
            gradePoints,
            "System");
    }

    private static decimal? GetGradePoints(decimal percentage)
    {
        // Standard grade points mapping
        if (percentage >= 90) return 10.0m;
        if (percentage >= 80) return 9.0m;
        if (percentage >= 70) return 8.0m;
        if (percentage >= 60) return 7.0m;
        if (percentage >= 55) return 6.0m;
        if (percentage >= 50) return 5.0m;
        if (percentage >= 45) return 4.0m;
        if (percentage >= 40) return 4.0m;
        return 0.0m;
    }
}

