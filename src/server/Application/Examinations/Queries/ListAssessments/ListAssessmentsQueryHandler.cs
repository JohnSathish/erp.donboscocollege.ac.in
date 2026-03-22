using ERP.Application.Admissions.Interfaces;
using ERP.Application.Academics.Interfaces;
using ERP.Application.Examinations.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Examinations.Queries.ListAssessments;

public class ListAssessmentsQueryHandler : IRequestHandler<ListAssessmentsQuery, IReadOnlyCollection<AssessmentSummaryDto>>
{
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IAcademicTermRepository _academicTermRepository;
    private readonly ILogger<ListAssessmentsQueryHandler> _logger;

    public ListAssessmentsQueryHandler(
        IAssessmentRepository assessmentRepository,
        ICourseRepository courseRepository,
        IAcademicTermRepository academicTermRepository,
        ILogger<ListAssessmentsQueryHandler> logger)
    {
        _assessmentRepository = assessmentRepository;
        _courseRepository = courseRepository;
        _academicTermRepository = academicTermRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<AssessmentSummaryDto>> Handle(
        ListAssessmentsQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Domain.Examinations.Entities.Assessment> assessments;

        if (request.ClassSectionId.HasValue)
        {
            assessments = await _assessmentRepository.GetByClassSectionAsync(
                request.ClassSectionId.Value,
                cancellationToken);
        }
        else if (request.CourseId.HasValue && request.AcademicTermId.HasValue)
        {
            assessments = await _assessmentRepository.GetByCourseAndTermAsync(
                request.CourseId.Value,
                request.AcademicTermId.Value,
                cancellationToken);
        }
        else if (request.AcademicTermId.HasValue)
        {
            assessments = await _assessmentRepository.GetByAcademicTermAsync(
                request.AcademicTermId.Value,
                cancellationToken);
        }
        else
        {
            throw new ArgumentException("At least one filter parameter (CourseId, AcademicTermId, or ClassSectionId) must be provided.");
        }

        // Filter by status if provided
        if (!string.IsNullOrWhiteSpace(request.Status) &&
            Enum.TryParse<Domain.Examinations.Entities.AssessmentStatus>(request.Status, true, out var status))
        {
            assessments = assessments.Where(a => a.Status == status).ToList();
        }

        // Get related entities
        var courseIds = assessments.Select(a => a.CourseId).Distinct().ToList();
        var termIds = assessments.Select(a => a.AcademicTermId).Distinct().ToList();

        var courses = new Dictionary<Guid, string>();
        var terms = new Dictionary<Guid, string>();

        foreach (var courseId in courseIds)
        {
            var course = await _courseRepository.GetByIdAsync(courseId, cancellationToken);
            if (course != null)
            {
                courses[courseId] = course.Name;
            }
        }

        foreach (var termId in termIds)
        {
            var term = await _academicTermRepository.GetByIdAsync(termId, cancellationToken);
            if (term != null)
            {
                terms[termId] = $"{term.AcademicYear} - {term.TermName}";
            }
        }

        return assessments
            .Select(a => new AssessmentSummaryDto(
                a.Id,
                a.CourseId,
                courses.GetValueOrDefault(a.CourseId, "Unknown"),
                a.AcademicTermId,
                terms.GetValueOrDefault(a.AcademicTermId, "Unknown"),
                a.Name,
                a.Code,
                a.Type.ToString(),
                a.ScheduledDate,
                a.MaxMarks,
                a.Status.ToString(),
                a.IsPublished))
            .ToList();
    }
}

