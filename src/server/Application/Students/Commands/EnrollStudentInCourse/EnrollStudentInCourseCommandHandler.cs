using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Commands.EnrollStudentInCourse;

public sealed class EnrollStudentInCourseCommandHandler : IRequestHandler<EnrollStudentInCourseCommand, EnrollStudentInCourseResult>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IAcademicHistoryRepository _academicHistoryRepository;
    private readonly ILogger<EnrollStudentInCourseCommandHandler> _logger;

    public EnrollStudentInCourseCommandHandler(
        IStudentRepository studentRepository,
        IAcademicHistoryRepository academicHistoryRepository,
        ILogger<EnrollStudentInCourseCommandHandler> logger)
    {
        _studentRepository = studentRepository;
        _academicHistoryRepository = academicHistoryRepository;
        _logger = logger;
    }

    public async Task<EnrollStudentInCourseResult> Handle(EnrollStudentInCourseCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            return new EnrollStudentInCourseResult(
                Guid.Empty,
                request.StudentId,
                request.CourseId,
                false,
                "Student not found.");
        }

        // Check if already enrolled
        var existingEnrollments = await _academicHistoryRepository.GetCourseEnrollmentsByStudentIdAsync(
            request.StudentId,
            request.TermId,
            cancellationToken);

        if (existingEnrollments.Any(e => e.CourseId == request.CourseId && !e.IsCompleted))
        {
            return new EnrollStudentInCourseResult(
                Guid.Empty,
                request.StudentId,
                request.CourseId,
                false,
                "Student is already enrolled in this course for the specified term.");
        }

        try
        {
            var enrollment = new CourseEnrollment(
                request.StudentId,
                request.CourseId,
                request.TermId,
                request.EnrollmentType,
                request.CreatedBy);

            await _academicHistoryRepository.AddCourseEnrollmentAsync(enrollment, cancellationToken);

            _logger.LogInformation(
                "Enrolled student {StudentId} in course {CourseId}, EnrollmentType: {EnrollmentType}",
                request.StudentId,
                request.CourseId,
                request.EnrollmentType);

            return new EnrollStudentInCourseResult(
                enrollment.Id,
                enrollment.StudentId,
                enrollment.CourseId,
                true);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to enroll student {StudentId} in course {CourseId}",
                request.StudentId,
                request.CourseId);

            return new EnrollStudentInCourseResult(
                Guid.Empty,
                request.StudentId,
                request.CourseId,
                false,
                ex.Message);
        }
    }
}

