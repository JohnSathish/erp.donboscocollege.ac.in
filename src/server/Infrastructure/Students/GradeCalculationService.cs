using ERP.Application.Admissions.Interfaces;
using ERP.Application.Students.Interfaces;
using ERP.Domain.Students.Entities;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Students;

public class GradeCalculationService : IGradeCalculationService
{
    private readonly IAcademicHistoryRepository _academicHistoryRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<GradeCalculationService> _logger;

    // Grade scale: Percentage -> Grade -> Grade Points
    private static readonly Dictionary<string, (decimal MinPercentage, decimal MaxPercentage, decimal GradePoints)> GradeScale = new()
    {
        { "A+", (90m, 100m, 10.0m) },
        { "A", (80m, 89.99m, 9.0m) },
        { "B+", (70m, 79.99m, 8.0m) },
        { "B", (60m, 69.99m, 7.0m) },
        { "C+", (50m, 59.99m, 6.0m) },
        { "C", (40m, 49.99m, 5.0m) },
        { "D", (35m, 39.99m, 4.0m) },
        { "F", (0m, 34.99m, 0.0m) }
    };

    public GradeCalculationService(
        IAcademicHistoryRepository academicHistoryRepository,
        ICourseRepository courseRepository,
        ILogger<GradeCalculationService> logger)
    {
        _academicHistoryRepository = academicHistoryRepository;
        _courseRepository = courseRepository;
        _logger = logger;
    }

    public async Task<GradeCalculationResult> CalculateGPAForAcademicRecordAsync(
        Guid academicRecordId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var academicRecord = await _academicHistoryRepository.GetAcademicRecordByIdAsync(academicRecordId, cancellationToken);
            if (academicRecord == null)
            {
                return new GradeCalculationResult(null, null, null, 0, 0, false, "Academic record not found.");
            }

            var enrollments = await _academicHistoryRepository.GetCourseEnrollmentsByStudentIdAsync(
                academicRecord.StudentId,
                academicRecord.TermId,
                cancellationToken);

            // Filter enrollments linked to this academic record
            var linkedEnrollments = new List<CourseEnrollmentData>();
            foreach (var enrollment in enrollments.Where(e => e.AcademicRecordId == academicRecordId))
            {
                var course = await _courseRepository.GetByIdAsync(enrollment.CourseId, cancellationToken);
                linkedEnrollments.Add(new CourseEnrollmentData(
                    enrollment.Id,
                    enrollment.MarksObtained,
                    enrollment.MaxMarks,
                    course?.CreditHours));
            }

            var gpa = CalculateGPAFromCourseEnrollments(linkedEnrollments);
            var totalCredits = linkedEnrollments.Count; // Placeholder - should sum actual credit hours
            var creditsEarned = linkedEnrollments.Count(e => e.MarksObtained.HasValue && e.MaxMarks.HasValue && 
                (e.MarksObtained.Value / e.MaxMarks.Value) >= 0.4m); // 40% passing

            var grade = gpa.HasValue ? CalculateGradeFromGPA(gpa.Value) : null;

            return new GradeCalculationResult(gpa, null, grade, totalCredits, creditsEarned, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to calculate GPA for academic record {AcademicRecordId}", academicRecordId);
            return new GradeCalculationResult(null, null, null, 0, 0, false, ex.Message);
        }
    }

    public async Task<GradeCalculationResult> CalculateCGPAForStudentAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var academicRecords = await _academicHistoryRepository.GetAcademicRecordsByStudentIdAsync(studentId, cancellationToken);
            var enrollments = await _academicHistoryRepository.GetCourseEnrollmentsByStudentIdAsync(studentId, null, cancellationToken);

            var enrollmentData = new List<CourseEnrollmentData>();
            foreach (var enrollment in enrollments.Where(e => e.IsCompleted && e.MarksObtained.HasValue && e.MaxMarks.HasValue))
            {
                var course = await _courseRepository.GetByIdAsync(enrollment.CourseId, cancellationToken);
                enrollmentData.Add(new CourseEnrollmentData(
                    enrollment.Id,
                    enrollment.MarksObtained,
                    enrollment.MaxMarks,
                    course?.CreditHours));
            }

            var cgpa = CalculateGPAFromCourseEnrollments(enrollmentData);
            var totalCredits = enrollmentData.Count;
            var creditsEarned = enrollmentData.Count(e => 
                e.MarksObtained.HasValue && e.MaxMarks.HasValue && 
                (e.MarksObtained.Value / e.MaxMarks.Value) >= 0.4m);

            var grade = cgpa.HasValue ? CalculateGradeFromGPA(cgpa.Value) : null;

            return new GradeCalculationResult(null, cgpa, grade, totalCredits, creditsEarned, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to calculate CGPA for student {StudentId}", studentId);
            return new GradeCalculationResult(null, null, null, 0, 0, false, ex.Message);
        }
    }

    public string? CalculateGradeFromPercentage(decimal percentage)
    {
        foreach (var (grade, (min, max, _)) in GradeScale)
        {
            if (percentage >= min && percentage <= max)
            {
                return grade;
            }
        }
        return "F"; // Default to fail if below minimum
    }

    public decimal? CalculateGPAFromCourseEnrollments(IEnumerable<CourseEnrollmentData> enrollments)
    {
        var validEnrollments = enrollments
            .Where(e => e.MarksObtained.HasValue && e.MaxMarks.HasValue && e.MaxMarks.Value > 0)
            .ToList();

        if (!validEnrollments.Any())
        {
            return null;
        }

        decimal totalGradePoints = 0;
        int totalCredits = 0;

        foreach (var enrollment in validEnrollments)
        {
            var percentage = (enrollment.MarksObtained!.Value / enrollment.MaxMarks!.Value) * 100;
            var grade = CalculateGradeFromPercentage(percentage);
            var gradePoints = GradeScale[grade!].GradePoints;
            var creditHours = enrollment.CreditHours ?? 1; // Default to 1 if not specified

            totalGradePoints += gradePoints * creditHours;
            totalCredits += creditHours;
        }

        return totalCredits > 0 ? totalGradePoints / totalCredits : null;
    }

    private string? CalculateGradeFromGPA(decimal gpa)
    {
        // Convert GPA to grade based on grade points
        if (gpa >= 9.0m) return "A+";
        if (gpa >= 8.0m) return "A";
        if (gpa >= 7.0m) return "B+";
        if (gpa >= 6.0m) return "B";
        if (gpa >= 5.0m) return "C+";
        if (gpa >= 4.0m) return "C";
        if (gpa >= 3.5m) return "D";
        return "F";
    }
}

