using ERP.Application.Admissions.Interfaces;
using ERP.Application.Students.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.Students.Queries.GetStudentDashboard;

public sealed class GetStudentDashboardQueryHandler : IRequestHandler<GetStudentDashboardQuery, StudentDashboardDto?>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IGuardianRepository _guardianRepository;
    private readonly IAcademicHistoryRepository _academicHistoryRepository;
    private readonly IGradeCalculationService _gradeCalculationService;
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<GetStudentDashboardQueryHandler> _logger;

    public GetStudentDashboardQueryHandler(
        IStudentRepository studentRepository,
        IGuardianRepository guardianRepository,
        IAcademicHistoryRepository academicHistoryRepository,
        IGradeCalculationService gradeCalculationService,
        ICourseRepository courseRepository,
        ILogger<GetStudentDashboardQueryHandler> _logger)
    {
        _studentRepository = studentRepository;
        _guardianRepository = guardianRepository;
        _academicHistoryRepository = academicHistoryRepository;
        _gradeCalculationService = gradeCalculationService;
        _courseRepository = courseRepository;
        this._logger = _logger;
    }

    public async Task<StudentDashboardDto?> Handle(GetStudentDashboardQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            return null;
        }

        // Get guardians
        var guardians = await _guardianRepository.GetByStudentIdAsync(request.StudentId, cancellationToken);
        var guardianDtos = guardians.Select(g => new GuardianInfoDto(
            g.Id,
            g.Name,
            g.Relationship,
            g.ContactNumber,
            g.Email,
            g.IsPrimary)).ToList();

        // Get academic records and enrollments
        var academicRecords = await _academicHistoryRepository.GetAcademicRecordsByStudentIdAsync(request.StudentId, cancellationToken);
        var courseEnrollments = await _academicHistoryRepository.GetCourseEnrollmentsByStudentIdAsync(request.StudentId, null, cancellationToken);

        // Get current semester GPA (most recent academic record)
        var currentRecord = academicRecords.OrderByDescending(r => r.AcademicYear).ThenByDescending(r => r.Semester).FirstOrDefault();
        var currentGPA = currentRecord?.GPA;

        // Calculate overall CGPA
        var cgpaResult = await _gradeCalculationService.CalculateCGPAForStudentAsync(request.StudentId, cancellationToken);

        // Get recent courses (last 5)
        var recentCourses = new List<RecentCourseDto>();
        foreach (var enrollment in courseEnrollments.OrderByDescending(e => e.EnrolledOnUtc).Take(5))
        {
            var course = await _courseRepository.GetByIdAsync(enrollment.CourseId, cancellationToken);
            recentCourses.Add(new RecentCourseDto(
                enrollment.Id,
                enrollment.CourseId,
                course?.Name,
                enrollment.Grade,
                enrollment.MarksObtained,
                enrollment.MaxMarks,
                enrollment.ResultStatus,
                enrollment.IsCompleted));
        }

        var academicSummary = new AcademicSummaryDto(
            currentGPA,
            cgpaResult.CGPA,
            currentRecord?.Grade,
            academicRecords.Sum(r => r.TotalCredits),
            academicRecords.Sum(r => r.CreditsEarned),
            courseEnrollments.Count,
            courseEnrollments.Count(e => e.IsCompleted));

        var profile = new StudentProfileDto(
            student.Id,
            student.StudentNumber,
            student.FullName,
            student.Email,
            student.MobileNumber,
            student.PhotoUrl,
            student.Shift,
            student.AcademicYear,
            student.ProgramCode,
            student.MajorSubject,
            student.Status.ToString(),
            student.EnrollmentDate);

        // Placeholder for notifications - should come from notification service
        var notifications = new List<DashboardNotificationDto>();

        return new StudentDashboardDto(
            profile,
            academicSummary,
            recentCourses,
            guardianDtos,
            notifications);
    }
}

