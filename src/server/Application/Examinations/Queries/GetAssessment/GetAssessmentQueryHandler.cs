using ERP.Application.Admissions.Interfaces;
using ERP.Application.Academics.Interfaces;
using ERP.Application.Examinations.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Examinations.Queries.GetAssessment;

public class GetAssessmentQueryHandler : IRequestHandler<GetAssessmentQuery, AssessmentDto?>
{
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IAcademicTermRepository _academicTermRepository;
    private readonly IClassSectionRepository _classSectionRepository;
    private readonly ILogger<GetAssessmentQueryHandler> _logger;

    public GetAssessmentQueryHandler(
        IAssessmentRepository assessmentRepository,
        ICourseRepository courseRepository,
        IAcademicTermRepository academicTermRepository,
        IClassSectionRepository classSectionRepository,
        ILogger<GetAssessmentQueryHandler> logger)
    {
        _assessmentRepository = assessmentRepository;
        _courseRepository = courseRepository;
        _academicTermRepository = academicTermRepository;
        _classSectionRepository = classSectionRepository;
        _logger = logger;
    }

    public async Task<AssessmentDto?> Handle(GetAssessmentQuery request, CancellationToken cancellationToken)
    {
        var assessment = await _assessmentRepository.GetByIdWithComponentsAsync(request.AssessmentId, cancellationToken);
        if (assessment == null)
        {
            return null;
        }

        // Get related entities
        var course = await _courseRepository.GetByIdAsync(assessment.CourseId, cancellationToken);
        var term = await _academicTermRepository.GetByIdAsync(assessment.AcademicTermId, cancellationToken);
        
        string? classSectionName = null;
        if (assessment.ClassSectionId.HasValue)
        {
            var section = await _classSectionRepository.GetByIdAsync(assessment.ClassSectionId.Value, cancellationToken);
            classSectionName = section?.SectionName;
        }

        var components = assessment.Components
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new AssessmentComponentDto(
                c.Id,
                c.Name,
                c.Code,
                c.MaxMarks,
                c.PassingMarks,
                c.Weightage,
                c.DisplayOrder,
                c.Instructions))
            .ToList();

        return new AssessmentDto(
            assessment.Id,
            assessment.CourseId,
            course?.Name ?? "Unknown",
            assessment.AcademicTermId,
            term != null ? $"{term.AcademicYear} - {term.TermName}" : "Unknown",
            assessment.ClassSectionId,
            classSectionName,
            assessment.Name,
            assessment.Code,
            assessment.Type.ToString(),
            assessment.ScheduledDate,
            assessment.Duration,
            assessment.TotalWeightage,
            assessment.MaxMarks,
            assessment.PassingMarks,
            assessment.Instructions,
            assessment.Status.ToString(),
            assessment.IsPublished,
            assessment.PublishedOnUtc,
            assessment.PublishedBy,
            components);
    }
}

