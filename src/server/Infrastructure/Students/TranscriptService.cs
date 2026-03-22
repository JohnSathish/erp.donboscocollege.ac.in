using ERP.Application.Admissions.Interfaces;
using ERP.Application.Students.Interfaces;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ERP.Infrastructure.Students;

public class TranscriptService : ITranscriptService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IAcademicHistoryRepository _academicHistoryRepository;
    private readonly IGradeCalculationService _gradeCalculationService;
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<TranscriptService> _logger;
    private static bool _questPdfLicenseConfigured;

    public TranscriptService(
        IStudentRepository studentRepository,
        IAcademicHistoryRepository academicHistoryRepository,
        IGradeCalculationService gradeCalculationService,
        ICourseRepository courseRepository,
        ILogger<TranscriptService> logger)
    {
        _studentRepository = studentRepository;
        _academicHistoryRepository = academicHistoryRepository;
        _gradeCalculationService = gradeCalculationService;
        _courseRepository = courseRepository;
        _logger = logger;
        ConfigureQuestPdfLicense();
    }

    private static void ConfigureQuestPdfLicense()
    {
        if (_questPdfLicenseConfigured)
            return;

        lock (typeof(TranscriptService))
        {
            if (_questPdfLicenseConfigured)
                return;

            QuestPDF.Settings.License = LicenseType.Community;
            _questPdfLicenseConfigured = true;
        }
    }

    public async Task<TranscriptDto> GenerateTranscriptAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student == null)
        {
            throw new InvalidOperationException($"Student with ID '{studentId}' not found.");
        }

        var academicRecords = await _academicHistoryRepository.GetAcademicRecordsByStudentIdAsync(studentId, cancellationToken);
        var courseEnrollments = await _academicHistoryRepository.GetCourseEnrollmentsByStudentIdAsync(studentId, null, cancellationToken);

        // Calculate overall CGPA
        var cgpaResult = await _gradeCalculationService.CalculateCGPAForStudentAsync(studentId, cancellationToken);

        var semesterDtos = new List<TranscriptSemesterDto>();

        foreach (var record in academicRecords.OrderBy(r => r.AcademicYear).ThenBy(r => r.Semester))
        {
            var recordEnrollments = courseEnrollments
                .Where(e => e.AcademicRecordId == record.Id && e.IsCompleted)
                .ToList();

            var courseDtos = new List<TranscriptCourseDto>();
            foreach (var enrollment in recordEnrollments)
            {
                var course = await _courseRepository.GetByIdAsync(enrollment.CourseId, cancellationToken);
                courseDtos.Add(new TranscriptCourseDto(
                    course?.Code ?? "N/A",
                    course?.Name ?? "Unknown Course",
                    enrollment.MarksObtained,
                    enrollment.MaxMarks,
                    enrollment.Grade,
                    enrollment.ResultStatus,
                    course?.CreditHours));
            }

            semesterDtos.Add(new TranscriptSemesterDto(
                record.AcademicYear,
                record.Semester,
                courseDtos,
                record.GPA,
                record.Grade,
                record.TotalCredits,
                record.CreditsEarned,
                record.ResultStatus));
        }

        return new TranscriptDto(
            student.Id,
            student.StudentNumber,
            student.FullName,
            student.DateOfBirth,
            student.Gender,
            student.ProgramCode,
            student.MajorSubject,
            student.MinorSubject,
            student.AcademicYear,
            student.EnrollmentDate,
            semesterDtos,
            cgpaResult.CGPA,
            cgpaResult.Grade,
            DateTime.UtcNow);
    }

    public async Task<byte[]> GenerateTranscriptPdfAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var transcript = await GenerateTranscriptAsync(studentId, cancellationToken);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .AlignCenter()
                    .PaddingBottom(0.5f, Unit.Centimetre)
                    .Text("TRANSCRIPT OF RECORDS")
                    .FontSize(16)
                    .Bold();

                page.Content()
                    .Column(column =>
                    {
                        column.Spacing(0.5f, Unit.Centimetre);

                        // Student Information
                        column.Item().PaddingBottom(0.5f, Unit.Centimetre).Column(info =>
                        {
                            info.Item().Text($"Student Number: {transcript.StudentNumber}").Bold();
                            info.Item().Text($"Name: {transcript.FullName}");
                            info.Item().Text($"Date of Birth: {transcript.DateOfBirth:dd MMM yyyy}");
                            info.Item().Text($"Gender: {transcript.Gender}");
                            if (!string.IsNullOrWhiteSpace(transcript.ProgramCode))
                                info.Item().Text($"Program: {transcript.ProgramCode}");
                            if (!string.IsNullOrWhiteSpace(transcript.MajorSubject))
                                info.Item().Text($"Major Subject: {transcript.MajorSubject}");
                            if (!string.IsNullOrWhiteSpace(transcript.MinorSubject))
                                info.Item().Text($"Minor Subject: {transcript.MinorSubject}");
                            info.Item().Text($"Academic Year: {transcript.AcademicYear}");
                            info.Item().Text($"Enrollment Date: {transcript.EnrollmentDate:dd MMM yyyy}");
                        });

                        column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        // Overall Performance
                        if (transcript.OverallCGPA.HasValue)
                        {
                            column.Item().PaddingVertical(0.5f, Unit.Centimetre).Column(overall =>
                            {
                                overall.Item().Text("Overall Performance").Bold().FontSize(12);
                                overall.Item().Text($"Cumulative GPA (CGPA): {transcript.OverallCGPA:F2}");
                                if (!string.IsNullOrWhiteSpace(transcript.OverallGrade))
                                    overall.Item().Text($"Overall Grade: {transcript.OverallGrade}");
                            });

                            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
                        }

                        // Semester-wise Results
                        column.Item().PaddingTop(0.5f, Unit.Centimetre).Column(semesters =>
                        {
                            semesters.Item().PaddingBottom(0.3f, Unit.Centimetre).Text("Academic Records").Bold().FontSize(12);

                            foreach (var semester in transcript.Semesters)
                            {
                                semesters.Item().PaddingBottom(0.5f, Unit.Centimetre).Column(sem =>
                                {
                                    sem.Item().Text($"{semester.AcademicYear} - {semester.Semester}").Bold().FontSize(11);
                                    
                                    if (semester.GPA.HasValue)
                                    {
                                        sem.Item().Text($"GPA: {semester.GPA:F2}");
                                    }
                                    if (!string.IsNullOrWhiteSpace(semester.Grade))
                                    {
                                        sem.Item().Text($"Grade: {semester.Grade}");
                                    }
                                    sem.Item().Text($"Credits: {semester.CreditsEarned} / {semester.TotalCredits}");
                                    if (!string.IsNullOrWhiteSpace(semester.ResultStatus))
                                    {
                                        sem.Item().Text($"Status: {semester.ResultStatus}");
                                    }

                                    // Course Details Table
                                    if (semester.Courses.Any())
                                    {
                                        sem.Item().PaddingTop(0.2f, Unit.Centimetre).Table(table =>
                                        {
                                            table.ColumnsDefinition(columns =>
                                            {
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(1);
                                                columns.RelativeColumn(1);
                                                columns.RelativeColumn(1);
                                                columns.RelativeColumn(1);
                                            });

                                            table.Header(header =>
                                            {
                                                header.Cell().Element(CellStyle).Text("Course Code").Bold();
                                                header.Cell().Element(CellStyle).Text("Course Name").Bold();
                                                header.Cell().Element(CellStyle).Text("Credits").Bold();
                                                header.Cell().Element(CellStyle).Text("Marks").Bold();
                                                header.Cell().Element(CellStyle).Text("Grade").Bold();
                                                header.Cell().Element(CellStyle).Text("Status").Bold();
                                            });

                                            foreach (var course in semester.Courses)
                                            {
                                                table.Cell().Element(CellStyle).Text(course.CourseCode);
                                                table.Cell().Element(CellStyle).Text(course.CourseName);
                                                table.Cell().Element(CellStyle).Text(course.CreditHours?.ToString() ?? "-");
                                                
                                                var marksText = course.MarksObtained.HasValue && course.MaxMarks.HasValue
                                                    ? $"{course.MarksObtained:F0}/{course.MaxMarks:F0}"
                                                    : "-";
                                                table.Cell().Element(CellStyle).Text(marksText);
                                                
                                                table.Cell().Element(CellStyle).Text(course.Grade ?? "-");
                                                table.Cell().Element(CellStyle).Text(course.ResultStatus ?? "-");
                                            }
                                        });
                                    }
                                });

                                semesters.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);
                            }
                        });

                        // Footer
                        column.Item().PaddingTop(1, Unit.Centimetre).AlignRight().Text($"Generated on: {transcript.GeneratedOnUtc:dd MMM yyyy HH:mm} UTC").FontSize(8).FontColor(Colors.Grey.Medium);
                    });
            });
        });

        return document.GeneratePdf();
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container
            .Border(0.5f)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(5)
            .AlignMiddle();
    }
}

