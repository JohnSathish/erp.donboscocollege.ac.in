using ERP.Application.Students.Interfaces;
using ERP.Application.Students.Queries.GetStudentById;
using ERP.Application.Students.ViewModels;
using MediatR;

namespace ERP.Application.Students.Queries.GetStudentById;

public sealed class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDto?>
{
    private readonly IStudentRepository _repository;

    public GetStudentByIdQueryHandler(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<StudentDto?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (student == null)
        {
            return null;
        }

        return new StudentDto(
            student.Id,
            student.StudentNumber,
            student.FullName,
            student.DateOfBirth,
            student.Gender,
            student.Email,
            student.MobileNumber,
            student.PhotoUrl,
            student.ProgramId,
            student.ProgramCode,
            student.MajorSubject,
            student.MinorSubject,
            student.Shift,
            student.AcademicYear,
            student.AdmissionNumber,
            student.EnrollmentDate,
            student.Status.ToString(),
            student.CreatedOnUtc,
            student.CreatedBy,
            student.UpdatedOnUtc,
            student.UpdatedBy);
    }
}


