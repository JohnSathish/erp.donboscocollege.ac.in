using ERP.Application.Students.Interfaces;
using MediatR;

namespace ERP.Application.Students.Queries.GetStudentAcademicHistory;

public sealed class GetStudentAcademicHistoryQueryHandler : IRequestHandler<GetStudentAcademicHistoryQuery, StudentAcademicHistoryDto?>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IAcademicHistoryRepository _academicHistoryRepository;

    public GetStudentAcademicHistoryQueryHandler(
        IStudentRepository studentRepository,
        IAcademicHistoryRepository academicHistoryRepository)
    {
        _studentRepository = studentRepository;
        _academicHistoryRepository = academicHistoryRepository;
    }

    public async Task<StudentAcademicHistoryDto?> Handle(GetStudentAcademicHistoryQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
        {
            return null;
        }

        var academicRecords = await _academicHistoryRepository.GetAcademicRecordsByStudentIdAsync(request.StudentId, cancellationToken);
        var courseEnrollments = await _academicHistoryRepository.GetCourseEnrollmentsByStudentIdAsync(request.StudentId, null, cancellationToken);

        var academicRecordDtos = academicRecords.Select(r => new AcademicRecordDto(
            r.Id,
            r.AcademicYear,
            r.Semester,
            r.TermId,
            r.GPA,
            r.CGPA,
            r.Grade,
            r.ResultStatus,
            r.TotalCredits,
            r.CreditsEarned,
            r.Remarks,
            r.CreatedOnUtc)).ToList();

        var courseEnrollmentDtos = courseEnrollments.Select(e => new CourseEnrollmentDto(
            e.Id,
            e.CourseId,
            e.TermId,
            e.AcademicRecordId,
            e.EnrollmentType,
            e.EnrolledOnUtc,
            e.Grade,
            e.MarksObtained,
            e.MaxMarks,
            e.ResultStatus,
            e.IsCompleted,
            e.CompletedOnUtc,
            e.Remarks)).ToList();

        return new StudentAcademicHistoryDto(
            student.Id,
            student.StudentNumber,
            student.FullName,
            academicRecordDtos,
            courseEnrollmentDtos);
    }
}

